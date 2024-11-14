using UnityEngine;
using ViewModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using R3;
using Contracts;

namespace Infrastructure
{
    public class StoryLoadActionCmd : ICommand
    {
        private StoryLoadCmdDto _storyLoadCmdDto;

        public StoryLoadActionCmd(StoryLoadCmdDto storyLoadCmdDto)
        {
            _storyLoadCmdDto = storyLoadCmdDto;
        }

        public async void Execute()
        {
            try
            {
                Debug.Log("StoryLoadActionCmd Execute - Starting");
                _storyLoadCmdDto.gameManagerViewModel.actionIndications.Value = "Cargando historia...";

                var urlResult = _storyLoadCmdDto.gameManagerViewModel.currentPlayerStory.image_url;
                Debug.Log($"StoryLoadActionCmd Execute - URL: {urlResult}");

                // Step 1: Recognize the image
                Debug.Log("StoryLoadActionCmd Execute - Starting image recognition");
                var recognition = await RecognizeImage(urlResult);
                if (recognition.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {recognition.Exception}");
                    return;
                };
                var recognitionResult = recognition.Value;
                Debug.Log("StoryLoadActionCmd Execute - Image recognition completed");

                // Step 2: Generate the story
                Debug.Log("StoryLoadActionCmd Execute - Starting story generation");
                var story = await GenerateStory(recognitionResult, _storyLoadCmdDto.gameManagerViewModel.currentTopicData);
                if (story.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {story.Exception}");
                    return;
                };
                var storyResult = story.Value;
                Debug.Log("StoryLoadActionCmd Execute - Story generation completed");

                // Step 3: Generate the audio
                Debug.Log("StoryLoadActionCmd Execute - Starting audio generation");
                var audio = await GenerateAudio(storyResult);
                if (audio.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {audio.Exception}");
                    return;
                };
                Debug.Log("StoryLoadActionCmd Execute - Audio generation completed");
                _storyLoadCmdDto.gameManagerViewModel.actionIndications.Value = "Espere un momento...";

                // Step 4: Download the audio from the server
                Debug.Log("StoryLoadActionCmd Execute - Starting audio download");
                var voiceAudio = await DownloadAudio(audio.Value);
                if (voiceAudio.IsFailure)
                {
                    Debug.LogError($"PlayActionCmd failed: {voiceAudio.Exception}");
                    return;
                };

                var voiceAudioResult = voiceAudio.Value;
                Debug.Log("StoryLoadActionCmd Execute - Audio download completed");

                Debug.Log("StoryLoadActionCmd Execute - Starting story");
                await StartStory(storyResult.story_id, voiceAudioResult, storyResult, recognitionResult);
                Debug.Log("StoryLoadActionCmd Execute - Story started successfully");
            }
            catch (Exception ex)
            {
                _storyLoadCmdDto.gameManagerViewModel.isLoadingPlay.Value = false;
                _storyLoadCmdDto.gameManagerViewModel.isPlayingStory.Value = false;
                Debug.LogError($"PlayActionCmd failed: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async Task<Result<EnvironmentalImageAnalysisResponse>> RecognizeImage(string url)
        {
            // Step 3: Recognize the image
            var recognitionService = _storyLoadCmdDto.recognitionService;
            var recognitionResult = await _storyLoadCmdDto.ImageAnalysisGateway.GetAnalysis(recognitionService, url);
            Debug.Log(JsonConvert.SerializeObject(recognitionResult.Value));
            return recognitionResult;
        }

        private async Task<Result<StorytellerGptResponse>> GenerateStory(EnvironmentalImageAnalysisResponse recognitionResult, TopicDataViewModel topicDataViewModel)
        {
            // Step 4: Generate the story
            var topic = topicDataViewModel.keywords;
            var language = _storyLoadCmdDto.gameManagerViewModel.languageSelected.Value;
            var storyService = _storyLoadCmdDto.storytellerViewModel;
            var storyResult = await _storyLoadCmdDto.StorytellerGateway.GenerateStory(storyService, recognitionResult, topic, language);
            Debug.Log(JsonConvert.SerializeObject(storyResult.Value));
            return storyResult;
        }

        private async Task<Result<VoiceGeneratorResponse>> GenerateAudio(StorytellerGptResponse storyResult)
        {
            // Step 5: Get the audio from the story
            var story_id = storyResult.story_id;
            var language = _storyLoadCmdDto.gameManagerViewModel.languageSelected.Value;
            var voiceService = _storyLoadCmdDto.voiceGeneratorViewModel;
            var audioResult = await _storyLoadCmdDto.VoiceGeneratorGateway.GenerateVoice(voiceService, storyResult.story, story_id, language);
            Debug.Log(JsonConvert.SerializeObject(audioResult.Value));
            return audioResult;
        }

        private async Task<Result<AudioClip>> DownloadAudio(VoiceGeneratorResponse audioResult)
        {
            var voiceAudio = await _storyLoadCmdDto.AudioGateway.DownloadAudio(audioResult);
            return voiceAudio;
        }

        private async Task StartStory(string story_id, AudioClip voiceAudioResult, StorytellerGptResponse storyResult, EnvironmentalImageAnalysisResponse imageAnalysisResult)
        {
            var playerStory = _storyLoadCmdDto.gameManagerViewModel.currentPlayerStory;

            // Assign the audio clip and story
            playerStory.story_id = story_id;
            playerStory.audioClip = voiceAudioResult;
            playerStory.story = storyResult;
            playerStory.imageAnalysis = imageAnalysisResult;

            // Add the playerStory to the list
            if (_storyLoadCmdDto.gameManagerViewModel.playerStories == null)
            {
                _storyLoadCmdDto.gameManagerViewModel.playerStories = new List<PlayerStoriesViewModel>();
            }

            // Notify listeners that a new story has been added
            _storyLoadCmdDto.gameManagerViewModel.actionIndications.Value = "...";
            var title = playerStory.story.title;
            _storyLoadCmdDto.gameManagerViewModel.OnMessage.OnNext(new MessageDto(title, Color.yellow));
            var delay = _storyLoadCmdDto.gameManagerViewModel.storyDelay;
            await Task.Delay(delay);

            // Notify listeners that the story has started
            _storyLoadCmdDto.gameManagerViewModel.OnStoryStart.OnNext(playerStory);
        }
    }
}
