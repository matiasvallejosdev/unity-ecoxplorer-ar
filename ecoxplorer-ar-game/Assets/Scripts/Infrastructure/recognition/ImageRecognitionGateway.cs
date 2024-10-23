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
    public class ImageRecognitionGateway : IImageRecognitionGateway
    {
        public async Task<Result<EnvironmentalImageAnalysisResponse>> GetAnalysis(RecognitionServiceViewModel recognitionService, string imageUrl)
        {
            Debug.Log($"Starting image analysis for URL: {imageUrl}");
            try
            {
                var apiService = recognitionService.apiService;
                Debug.Log($"Using API service: {apiService.API_URL}");
                var response = await RecognizeImage(recognitionService, imageUrl);
                Debug.Log("Image analysis completed successfully");
                return Result<EnvironmentalImageAnalysisResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Recognition failed: {ex.Message}");
                return Result<EnvironmentalImageAnalysisResponse>.Failure(ex);
            }
        }

        private async Task<EnvironmentalImageAnalysisResponse> RecognizeImage(RecognitionServiceViewModel recognitionServiceViewModel, string imageUrl)
        {
            var apiService = recognitionServiceViewModel.apiService;
            var isMock = recognitionServiceViewModel.testMode;
            var endpoint = recognitionServiceViewModel.endpoint;
            string postUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/{(isMock ? $"{endpoint}-mock" : endpoint)}";
            Debug.Log($"Sending {(isMock ? "mock " : "")}request to: {postUrl}");
            var imageAnalysisRequest = new EnvironmentalImageAnalysisRequest
            {
                image_url = imageUrl
            };
            string jsonData = JsonConvert.SerializeObject(imageAnalysisRequest);
            using var www = new UnityWebRequest(postUrl, "POST");
            
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(apiService.API_KEY_HEADER, apiService.API_KEY);

            Debug.Log($"Sending web request for {(isMock ? "mock " : "")}image recognition");
            await www.SendWebRequestAsync();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{(isMock ? "Mock " : "")}Web request failed: {www.error}");
                throw new Exception($"Web request failed: {www.error}");
            }

            Debug.Log($"{(isMock ? "Mock " : "")}Web request successful, parsing response");
            var response = JObject.Parse(www.downloadHandler.text);
            return response.ToObject<EnvironmentalImageAnalysisResponse>();
        }
    }
}