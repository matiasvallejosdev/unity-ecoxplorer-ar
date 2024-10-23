using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "StorageService", menuName = "Services/Storage Service", order = 0)]
    public class StorageServiceViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public string BUCKET_NAME; // Your S3 Bucket name
        public string BUCKET_REGION; // Your AWS Region
    }
}