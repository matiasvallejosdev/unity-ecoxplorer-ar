using System;
using System.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure
{
    public class StorytellerGateway : IStorytellerGateway
    {
        public async Task<Result<StorytellerGptResponse>> GenerateStory(StorytellerViewModel storytellerViewModel, EnvironmentalImageAnalysisResponse imageAnalysisResponse, string topic, string language = "es")
        {
            Debug.Log($"Starting story generation for image analysis response");
            try
            {
                var apiService = storytellerViewModel.apiService;
                Debug.Log($"Using API service: {apiService.API_URL}");
                var response = await GetStorytellerGptResponseAsync(storytellerViewModel, imageAnalysisResponse, language, topic);
                Debug.Log("Story generation completed successfully");
                return Result<StorytellerGptResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Story generation failed: {ex.Message}");
                return Result<StorytellerGptResponse>.Failure(ex);
            }
        }

        private async Task<StorytellerGptResponse> GetStorytellerGptResponseAsync(StorytellerViewModel storytellerViewModel, EnvironmentalImageAnalysisResponse imageAnalysisResponse, string language, string topic)
        {
            var apiService = storytellerViewModel.apiService;
            var isMock = storytellerViewModel.testMode;
            var endpoint = storytellerViewModel.endpoint;
            string postUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/{(isMock ? $"{endpoint}-mock" : endpoint)}";
            Debug.Log($"Sending {(isMock ? "mock" : "")} request to: {postUrl}");
            var storytellerRequest = new StorytellerGptRequest
            {
                image_analysis = imageAnalysisResponse,
                language = language,
                topic = topic
            };
            string jsonData = JsonConvert.SerializeObject(storytellerRequest);
            Debug.Log($"{(isMock ? "Mock" : "")} Request body: {jsonData}");
            using var www = new UnityWebRequest(postUrl, "POST");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(apiService.API_KEY_HEADER, apiService.API_KEY);

            Debug.Log($"Sending {(isMock ? "mock" : "")} web request for story generation");
            await www.SendWebRequestAsync();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{(isMock ? "Mock" : "")} Web request failed: {www.error}");
                throw new Exception($"{(isMock ? "Mock" : "")} Web request failed: {www.error}");
            }

            Debug.Log($"{(isMock ? "Mock " : "")}Web request successful, parsing response");
            var response = JObject.Parse(www.downloadHandler.text);
            Debug.Log($"{(isMock ? "Mock " : "")}Response: {response}");
            return response.ToObject<StorytellerGptResponse>();
        }
    }
}