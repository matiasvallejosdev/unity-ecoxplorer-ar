using System;
using Newtonsoft.Json;

namespace ViewModel
{
    [Serializable]
    public class S3UploadResponse
    {
        public UrlData url { get; set; }
        public string key_file { get; set; }
        public int expires_in { get; set; }
        public string created_at { get; set; }
    }

    [Serializable]
    public class UrlData
    {
        public string url { get; set; }
        public Fields fields { get; set; }
    }

    [Serializable]
    public class Fields
    {
        public string key { get; set; }
        [JsonProperty("x-amz-algorithm")]
        public string XAmzAlgorithm { get; set; }
        [JsonProperty("x-amz-credential")]
        public string XAmzCredential { get; set; }
        [JsonProperty("x-amz-date")]
        public string XAmzDate { get; set; }
        [JsonProperty("x-amz-security-token")]
        public string XAmzSecurityToken { get; set; }
        public string policy { get; set; }
        [JsonProperty("x-amz-signature")]
        public string XAmzSignature { get; set; }
    }
}