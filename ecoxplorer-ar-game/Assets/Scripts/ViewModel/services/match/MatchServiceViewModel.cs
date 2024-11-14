using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "MatchService", menuName = "Services/Match Service", order = 0)]
    public class MatchServiceViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public bool testMode;
        public string endpoint = "image-match";
    }
}