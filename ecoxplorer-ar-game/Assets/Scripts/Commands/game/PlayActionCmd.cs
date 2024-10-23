using UnityEngine;
using ViewModel;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Components.Utils;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using R3;
using UnityEngine.Networking;
using System.IO;

namespace Infrastructure
{
    public class PlayActionCmd : ICommand
    {
        private PlayCmdDto _playCmdDto;

        public PlayActionCmd(PlayCmdDto playCmdDto)
        {
            _playCmdDto = playCmdDto;
        }

        public async void Execute()
        {
            _playCmdDto.gameManagerViewModel.isLoadingPlay.Value = true;

            try
            {
                // Step 1: Get the image from the camera
                var screenTextures = await CaptureImage();
                if (screenTextures.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {screenTextures.Exception}");
                    return;
                };
                var screenTexturesResult = screenTextures.Value;

                // Step 2: Upload the image to the server
                var url = await UploadImage(screenTexturesResult[0]);
                if (url.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {url.Exception}");
                    return;
                };
                var urlResult = url.Value;

                // Step 3: Recognize the image
                var recognition = await RecognizeImage(urlResult);
                if (recognition.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {recognition.Exception}");
                    return;
                };
                var recognitionResult = recognition.Value;

                // Step 4: Generate the story
                var story = await GenerateStory(recognitionResult);
                if (story.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {story.Exception}");
                    return;
                };
                var storyResult = story.Value;

                // Step 5: Generate the audio
                var audio = await GenerateAudio(storyResult);
                if (audio.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {audio.Exception}");
                    return;
                };

                // Step 6: Download the audio from the server
                var voiceAudio = await DownloadAudio(audio.Value);
                if (voiceAudio.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {voiceAudio.Exception}");
                    return;
                };

                var voiceAudioResult = voiceAudio.Value;
                StartStory(storyResult.story_id, recognitionResult.image_id, urlResult,
                voiceAudioResult, storyResult, recognitionResult, screenTexturesResult[1]);
            }
            catch (Exception ex)
            {
                _playCmdDto.gameManagerViewModel.isLoadingPlay.Value = false;
                _playCmdDto.gameManagerViewModel.isScanning.Value = false;
                _playCmdDto.gameManagerViewModel.isPlayingStory.Value = false;
                Debug.LogError($"PlayActionCmd failed: {ex.Message}");
            }
        }


        private Task<Result<List<Texture2D>>> CaptureImage()
        {
            return Task.FromResult(Result<List<Texture2D>>.Success(GetTexturesFromCamera()));
        }

        private async Task<Result<string>> UploadImage(Texture2D cropTexture)
        {
            // Step 2: Upload image to the server
            var storageService = _playCmdDto.storageService;
            var storageResult = await _playCmdDto.StorageImageGateway.UploadImageAndGetUrl(storageService, cropTexture);
            Debug.Log(JsonConvert.SerializeObject(storageResult.Value));
            return storageResult;
        }

        private async Task<Result<EnvironmentalImageAnalysisResponse>> RecognizeImage(string url)
        {
            // Step 3: Recognize the image
            var recognitionService = _playCmdDto.recognitionService;
            var recognitionResult = await _playCmdDto.ImageAnalysisGateway.GetAnalysis(recognitionService, url);
            Debug.Log(JsonConvert.SerializeObject(recognitionResult.Value));
            return recognitionResult;
        }

        private async Task<Result<StorytellerGptResponse>> GenerateStory(EnvironmentalImageAnalysisResponse recognitionResult)
        {
            // Step 4: Generate the story
            var language = _playCmdDto.gameManagerViewModel.languageSelected.Value;
            var storyService = _playCmdDto.storytellerViewModel;
            var storyResult = await _playCmdDto.StorytellerGateway.GenerateStory(storyService, recognitionResult, language);
            Debug.Log(JsonConvert.SerializeObject(storyResult.Value));
            return storyResult;
        }

        private async Task<Result<VoiceGeneratorResponse>> GenerateAudio(StorytellerGptResponse storyResult)
        {
            // Step 5: Get the audio from the story
            var story_id = storyResult.story_id;
            var language = _playCmdDto.gameManagerViewModel.languageSelected.Value;
            var voiceService = _playCmdDto.voiceGeneratorViewModel;
            var audioResult = await _playCmdDto.VoiceGeneratorGateway.GenerateVoice(voiceService, storyResult.story, story_id, language);
            Debug.Log(JsonConvert.SerializeObject(audioResult.Value));
            return audioResult;
        }


        private List<Texture2D> GetTexturesFromCamera()
        {
            var arCamera = _playCmdDto.arCamera;
            var gameSettings = _playCmdDto.gameManagerViewModel;
            Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);
            Texture2D textureCrop = Screenshot.ResampleAndCrop(screenShotTex, gameSettings.textureWidth, gameSettings.textureHeight);
            if (gameSettings.saveLocalScreenshoot)
            {
                Screenshot.SaveScreenshot(screenShotTex, "screenshot");
                Screenshot.SaveScreenshot(textureCrop, "screenshotcropped");
            }
            return new List<Texture2D> { textureCrop, screenShotTex };
        }

        private async Task<Result<AudioClip>> DownloadAudio(VoiceGeneratorResponse audioResult)
        {
            var voiceAudio = await _playCmdDto.AudioGateway.DownloadAudio(audioResult);
            return voiceAudio;
        }

        private void StartStory(string story_id, string image_id, string image_url, AudioClip voiceAudioResult, StorytellerGptResponse storyResult, EnvironmentalImageAnalysisResponse imageAnalysisResult, Texture2D imageResult)
        {
            // Create a new instance of PlayerStoriesViewModel
            var playerStory = ScriptableObject.CreateInstance<PlayerStoriesViewModel>();

            // Assign the audio clip and story
            playerStory.story_id = story_id;
            playerStory.audioClip = voiceAudioResult;
            playerStory.story = storyResult;
            playerStory.imageAnalysis = imageAnalysisResult;
            playerStory.image = imageResult;
            playerStory.image_id = image_id;
            playerStory.image_url = image_url;

            // Add the playerStory to the list
            if (_playCmdDto.gameManagerViewModel.playerStories == null)
            {
                _playCmdDto.gameManagerViewModel.playerStories = new List<PlayerStoriesViewModel>();
            }
            
            // Notify listeners that a new story has been added
            _playCmdDto.gameManagerViewModel.OnPlayStory.OnNext(playerStory);
        }

        //         recognitionManager.recognitionActive.Value = false;
        //         recognitionManager.OnRecognitionStart.OnNext(true);

        //         List<Texture2D> textures = GetTexture();
        //         Texture2D textureCrop = textures[0];

        //         recognitionManager.recognitionResponse.Value = new RecognitionResponse
        //         {
        //             recognition = "Uploading Image..",
        //             statusCode = "Wait a moment"
        //         };

        //         var storageResult = await storageImageGateway.PostImage(storageService, textureCrop);

        //         if (storageResult.IsFailure)
        //         {
        //             Debug.LogError("Image upload failed.");
        //             recognitionManager.recognitionResponse.Value = new RecognitionResponse
        //             {
        //                 recognition = "Failed to upload image.",
        //                 statusCode = "Error"
        //             };
        //             return;
        //         }

        //         recognitionManager.recognitionResponse.Value = new RecognitionResponse
        //         {
        //             recognition = "Processing Image..",
        //             statusCode = "Wait a moment"
        //         };

        //         var recognitionResult = await brandRecognitionGateway.GetRecogntion(null, storageResult.Value.imageUrl);

        //         if (recognitionResult.IsFailure)
        //         {
        //             Debug.LogError("Recognition failed.");
        //             recognitionManager.recognitionResponse.Value = new RecognitionResponse
        //             {
        //                 recognition = "Failed to process image.",
        //                 statusCode = "Error"
        //             };
        //             return;
        //         }

        //         string imageName = recognitionResult.Value.recognition;
        //         Debug.Log($"Image Name: {imageName}");

        //         await runtimeLibraryGateway.AddImageRuntimeLibrary(textureCrop, trackedImageManager, imageName);

        //         recognitionManager.recognitionResponse.Value = recognitionResult.Value;
        //         recognitionManager.recognitionActive.Value = true;
        //         recognitionManager.recognitionObjectsCount.Value++;
        //         recognitionManager.OnRecognitionStart.OnNext(false);
        //     }

        //     private List<Texture2D> GetTexture()
        //     {
        //         recognitionManager.recognitionResponse.Value = new RecognitionResponse
        //         {
        //             recognition = "Capturing Image..",
        //             statusCode = "Wait a moment"
        //         };

        //         Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);
        //         Texture2D textureCrop = Screenshot.ResampleAndCrop(screenShotTex, recognitionManager.textureWidth, recognitionManager.textureHeight);

        //         if (recognitionManager.saveLocalScreenshoot)
        //         {
        //             Screenshot.SaveScreenshot(screenShotTex);
        //             Screenshot.SaveScreenshot(textureCrop);
        //         }

        //         recognitionManager.lastScreenshoot.Value = textureCrop;

        //         return new List<Texture2D> { textureCrop, screenShotTex };
        //     }
    }
}
