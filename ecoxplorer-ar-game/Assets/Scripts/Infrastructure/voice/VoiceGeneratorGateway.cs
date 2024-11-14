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
    public class VoiceGeneratorGateway : IVoiceGeneratorGateway
    {
        public async Task<Result<VoiceGeneratorResponse>> GenerateVoice(VoiceGeneratorViewModel voiceGeneratorViewModel, string story, string story_id, string narrator)
        {
            Debug.Log($"Starting voice generation for story");
            try
            {
                var apiService = voiceGeneratorViewModel.apiService;
                var isMock = voiceGeneratorViewModel.testMode;
                var response = await GenerateVoice(apiService, voiceGeneratorViewModel, story, story_id, narrator);
                Debug.Log("Voice generation completed successfully");
                return Result<VoiceGeneratorResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Voice generation failed: {ex.Message}");
                return Result<VoiceGeneratorResponse>.Failure(ex);
            }
        }

        private async Task<VoiceGeneratorResponse> GenerateVoice(ApiServiceViewModel apiService, VoiceGeneratorViewModel voiceGeneratorViewModel, string story, string voice_id, string narrator)
        {
            var endpoint = voiceGeneratorViewModel.endpoint;
            var isMock = voiceGeneratorViewModel.testMode;
            string postUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/{(isMock ? $"{endpoint}-mock" : endpoint)}";
            Debug.Log($"Sending {(isMock ? "mock" : "")} request to: {postUrl}");
            var voiceGeneratorRequest = new VoiceGeneratorRequest
            {
                voice_id = voice_id,
                story = story,
                narrator = narrator
            };
            string jsonData = JsonConvert.SerializeObject(voiceGeneratorRequest);
            Debug.Log($"{(isMock ? "Mock" : "")} Request body: {jsonData}");

            using var www = new UnityWebRequest(postUrl, "POST");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(apiService.API_KEY_HEADER, apiService.API_KEY);

            Debug.Log($"Sending {(isMock ? "mock" : "")} web request for voice generation");
            await www.SendWebRequestAsync();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{(isMock ? "Mock" : "")} Web request failed: {www.error}");
                throw new Exception($"{(isMock ? "Mock" : "")} Web request failed: {www.error}");
            }

            var response = JObject.Parse(www.downloadHandler.text);
            Debug.Log($"{(isMock ? "Mock" : "")} Response: {response}");
            return response.ToObject<VoiceGeneratorResponse>();
        }
    }
}
