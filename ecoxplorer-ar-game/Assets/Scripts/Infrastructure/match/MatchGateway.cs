
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
    public class MatchGateway : IMatchGateway
    {
        public async Task<Result<ImageMatchResponse>> GetMatch(MatchServiceViewModel matchServiceViewModel, string topic, string url)
        {
            try
            {
                var response = await GetMatchFromApi(matchServiceViewModel, topic, url);
                return response;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error getting match: {ex.Message}");
                return Result<ImageMatchResponse>.Failure(ex);
            }
        }

        private async Task<Result<ImageMatchResponse>> GetMatchFromApi(MatchServiceViewModel matchServiceViewModel, string topic, string url)
        {
            var apiService = matchServiceViewModel.apiService;
            var isMock = matchServiceViewModel.testMode;
            var endpoint = matchServiceViewModel.endpoint;
            string postUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/{(isMock ? $"{endpoint}-mock" : endpoint)}";
            Debug.Log($"Sending {(isMock ? "mock " : "")}request to: {postUrl}");
            var imageMatchRequest = new ImageMatchRequest
            {
                image_url = url,
                topic = topic
            };
            string jsonData = JsonConvert.SerializeObject(imageMatchRequest);
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
                return Result<ImageMatchResponse>.Failure(new Exception($"Web request failed: {www.error}"));
            }

            Debug.Log($"{(isMock ? "Mock " : "")}Web request successful, parsing response");
            var response = JObject.Parse(www.downloadHandler.text);
            Debug.Log($"{(isMock ? "Mock " : "")}Response: {response}");
            return Result<ImageMatchResponse>.Success(response.ToObject<ImageMatchResponse>());
        }
    }
}