
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using R3;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure
{
    public class LevelBuilder : ILevelBuilderGateway
    {
        public async Task<Result<LevelBuilderResponse>> GetLevelBuilder(LevelBuilderServiceViewModel levelBuilderServiceViewModel, string topic, string language)
        {
            try
            {
                var response = await GetLevelBuilderFromApi(levelBuilderServiceViewModel, topic, language);
                return response;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error getting match: {ex.Message}");
                return Result<LevelBuilderResponse>.Failure(ex);
            }
        }

        private async Task<Result<LevelBuilderResponse>> GetLevelBuilderFromApi(LevelBuilderServiceViewModel levelBuilderServiceViewModel, string topic, string language)
        {
            var apiService = levelBuilderServiceViewModel.apiService;
            var isMock = levelBuilderServiceViewModel.testMode;
            var endpoint = levelBuilderServiceViewModel.endpoint;
            string postUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/{(isMock ? $"{endpoint}-mock" : endpoint)}";
            Debug.Log($"Sending {(isMock ? "mock " : "")}request to: {postUrl}");
            var levelBuilderRequest = new LevelBuilderRequest
            {
                topic = topic,
                language = language
            };
            string jsonData = JsonConvert.SerializeObject(levelBuilderRequest);
            using var www = new UnityWebRequest(postUrl, "POST");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(apiService.API_KEY_HEADER, apiService.API_KEY);

            Debug.Log($"Sending web request for {(isMock ? "mock " : "")}image match");
            await www.SendWebRequestAsync();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{(isMock ? "Mock " : "")}Web request failed: {www.error}");
                return Result<LevelBuilderResponse>.Failure(new Exception($"Web request failed: {www.error}"));
            }

            Debug.Log($"{(isMock ? "Mock " : "")}Web request successful, parsing response");
            var response = JObject.Parse(www.downloadHandler.text);
            Debug.Log($"{(isMock ? "Mock " : "")}Response: {response}");
            return Result<LevelBuilderResponse>.Success(response.ToObject<LevelBuilderResponse>());
        }
    }
}