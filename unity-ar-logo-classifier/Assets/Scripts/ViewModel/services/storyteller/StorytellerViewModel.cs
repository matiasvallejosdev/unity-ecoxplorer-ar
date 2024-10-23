using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "StorytellerService", menuName = "Services/Storyteller Service", order = 0)]
    public class StorytellerViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public bool testMode;
        public string endpoint = "storyteller";
    }
}