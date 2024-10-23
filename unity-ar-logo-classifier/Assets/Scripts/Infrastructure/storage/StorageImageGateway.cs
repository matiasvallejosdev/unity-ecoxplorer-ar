using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using R3;

namespace Infrastructure
{
    public class StorageImageGateway : IStorageImageGateway
    {
        public async Task<Result<string>> UploadImageAndGetUrl(StorageServiceViewModel storageService, Texture2D texture)
        {
            try
            {
                Debug.Log("Uploading image to S3...");
                Debug.Log($"S3 Bucket: {storageService.BUCKET_NAME}, Region: {storageService.BUCKET_REGION}");
                var key = $"unity_{DateTime.Now:yyyyMMddHHmmss}.png";
                var response = await GetS3UploadUrl(storageService.apiService, key);
                Debug.Log(JsonConvert.SerializeObject(response));
                var uploadResult = await UploadImageToS3(response, texture);
                var imageUrl = await GetS3ImageUrl(storageService.apiService, key);
                return Result<string>.Success(imageUrl);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex);
            }
        }

        private async Task<S3UploadResponse> GetS3UploadUrl(ApiServiceViewModel storageService, string key)
        {
            // Construct the URL for the GET request
            string getUrl = $"{storageService.API_URL}/{storageService.API_VERSION}/files/upload?key={Uri.EscapeDataString(key)}&expires_in=3000";

            // Create a GET request
            using var www = UnityWebRequest.Get(getUrl);

            // Set the necessary headers
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(storageService.API_KEY_HEADER, storageService.API_KEY);

            // Send the request
            await www.SendWebRequestAsync();

            if (www.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Web request failed: {www.error}");
            }

            // Deserialize the response
            var response = JsonConvert.DeserializeObject<S3UploadResponse>(www.downloadHandler.text);
            return response;
        }
        private async Task<bool> UploadImageToS3(S3UploadResponse s3uploadResponse, Texture2D texture)
        {
            Debug.Log("Starting UploadImageToS3 method");
            var postUrltoUpload = s3uploadResponse.url.url;
            Debug.Log($"Post URL to upload: {postUrltoUpload}");
            var fields = s3uploadResponse.url.fields;
            var fileName = s3uploadResponse.url.fields.key;
            Debug.Log($"File name: {fileName}");
            var form = AWSWebFormBuilder.BuildUploadRequest(fields, texture, fileName);
            Debug.Log("Form built for upload request");
            using var www = UnityWebRequest.Post(postUrltoUpload, form);
            Debug.Log("Sending web request for S3 upload");
            await www.SendWebRequestAsync();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"S3 upload failed: {www.error}");
                throw new Exception($"Web request failed: {www.error}");
            }
            Debug.Log("S3 upload completed successfully");
            return true;
        }

        private async Task<string> GetS3ImageUrl(ApiServiceViewModel apiService, string key)
        {
            string getUrl = $"{apiService.API_URL}/{apiService.API_VERSION}/files?key={Uri.EscapeDataString(key)}";
            using var www = UnityWebRequest.Get(getUrl);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader(apiService.API_KEY_HEADER, apiService.API_KEY);
            await www.SendWebRequestAsync();

            if (www.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Web request failed: {www.error}");
            }

            var response = JsonConvert.DeserializeObject<UrlData>(www.downloadHandler.text);
            return response.url;
        }
    }
}
