

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Utils;
using Contracts;
using Newtonsoft.Json;
using R3;
using UnityEngine;
using ViewModel;

namespace Commands
{
    public class MatchActionCmd : ICommand
    {
        private MatchCmdDto _matchCmdDto;

        public MatchActionCmd(MatchCmdDto matchCmdDto)
        {
            _matchCmdDto = matchCmdDto;
        }

        public async void Execute()
        {
            Debug.Log("MatchActionCmd executed");
            _matchCmdDto.gameManagerViewModel.isLoadingPlay.Value = true;
            _matchCmdDto.gameManagerViewModel.actionIndications.Value = "Manten presionado";
            _matchCmdDto.gameManagerViewModel.OnCapture.OnNext(true);
            try
            {
                if (!IsSessionRunning())
                    return;
                // Step 1: Get the image from the camera
                var screenTextures = await CaptureImage();
                if (screenTextures.IsFailure)
                {
                    _matchCmdDto.gameManagerViewModel.OnMatch.OnNext(new MatchReactiveDto { isMatch = false, isCancelledOperation = true });
                    Debug.LogError($"MatchActionCmd failed: {screenTextures.Exception}");
                    return;
                };
                var screenTexturesResult = screenTextures.Value;

                if (!IsSessionRunning())
                    return;
                // Step 2: Upload the image to the server
                var imageUuid = Guid.NewGuid().ToString();
                var url = await UploadImage(screenTexturesResult[0], imageUuid);
                if (url.IsFailure)
                {
                    _matchCmdDto.gameManagerViewModel.OnMatch.OnNext(new MatchReactiveDto { isMatch = false, isCancelledOperation = true });
                    Debug.LogError($"MatchActionCmd failed: {url.Exception}");
                    return;
                };
                var urlResult = url.Value;

                if (!IsSessionRunning())
                    return;

                // Step 3: Match the image with the current story
                var match = await GetMatch(urlResult);
                if (match.IsFailure)
                {
                    _matchCmdDto.gameManagerViewModel.OnMatch.OnNext(new MatchReactiveDto { isMatch = false, isCancelledOperation = true });
                    Debug.LogError($"MatchActionCmd failed: {match.Exception}");
                    return;
                };

                // Step 4: Save the match
                var matchResult = match.Value;
                if (matchResult.match)
                {
                    StartLoadStory(screenTexturesResult[0], imageUuid, urlResult);
                    _matchCmdDto.gameManagerViewModel.OnStoryLoad.OnNext(true);
                    _matchCmdDto.gameManagerViewModel.isLoadingPlay.Value = true;
                }
                else
                {
                    _matchCmdDto.gameManagerViewModel.isLoadingPlay.Value = false;
                }
                _matchCmdDto.gameManagerViewModel.OnMatch.OnNext(new MatchReactiveDto { isMatch = matchResult.match, isCancelledOperation = false, matchTexture = screenTexturesResult[0] });
            }
            catch (Exception ex)
            {
                _matchCmdDto.gameManagerViewModel.isLoadingPlay.Value = false;
                Debug.LogError($"MatchActionCmd failed: {ex.Message}");
            }
        }

        private bool IsSessionRunning()
        {
            var sessionRunning = _matchCmdDto.gameManagerViewModel.isSessionStarted.Value;
            if (!sessionRunning)
            {
                _matchCmdDto.gameManagerViewModel.isLoadingPlay.Value = false;
                _matchCmdDto.gameManagerViewModel.OnMatch.OnNext(new MatchReactiveDto { isMatch = false, isCancelledOperation = true });
            }
            return sessionRunning;
        }

        private void StartLoadStory(Texture2D imageResult, string image_id, string image_url)
        {
            var playerStory = ScriptableObject.CreateInstance<PlayerStoriesViewModel>();
            playerStory.image = imageResult;
            playerStory.image_id = image_id;
            playerStory.image_url = image_url;
            _matchCmdDto.gameManagerViewModel.currentPlayerStory = playerStory;
        }

        private List<Texture2D> GetTexturesFromCamera()
        {
            var arCamera = _matchCmdDto.arCamera;
            var gameSettings = _matchCmdDto.gameManagerViewModel;
            Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);
            Texture2D textureCrop = Screenshot.ResampleAndCrop(screenShotTex, gameSettings.textureWidth, gameSettings.textureHeight);
            if (gameSettings.saveLocalScreenshoot)
            {
                Screenshot.SaveScreenshot(screenShotTex, "screenshot");
                Screenshot.SaveScreenshot(textureCrop, "screenshotcropped");
            }
            return new List<Texture2D> { textureCrop, screenShotTex };
        }

        private Task<Result<List<Texture2D>>> CaptureImage()
        {
            return Task.FromResult(Result<List<Texture2D>>.Success(GetTexturesFromCamera()));
        }

        private async Task<Result<string>> UploadImage(Texture2D cropTexture, string imageUuid)
        {
            // Step 2: Upload image to the server
            var storageService = _matchCmdDto.storageService;
            var storageResult = await _matchCmdDto.StorageImageGateway.UploadImageAndGetUrl(storageService, cropTexture, imageUuid);
            Debug.Log(JsonConvert.SerializeObject(storageResult.Value));
            return storageResult;
        }

        private async Task<Result<ImageMatchResponse>> GetMatch(string url)
        {
            var topic = _matchCmdDto.gameManagerViewModel.currentTopicData;
            var topicDescription = topic.topic_description;
            var matchResult = await _matchCmdDto.MatchGateway.GetMatch(_matchCmdDto.matchService, topicDescription, url);
            return matchResult;
        }
    }
}