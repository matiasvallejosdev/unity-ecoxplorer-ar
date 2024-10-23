using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "RecognitionService", menuName = "Services/Recognition Service", order = 0)]
    public class RecognitionServiceViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public bool testMode;
        public string endpoint = "image-recognition";
    }
}