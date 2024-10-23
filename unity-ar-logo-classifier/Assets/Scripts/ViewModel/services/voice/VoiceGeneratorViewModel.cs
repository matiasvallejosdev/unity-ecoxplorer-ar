using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "VoiceGeneratorService", menuName = "Services/Voice Generator Service", order = 0)]
    public class VoiceGeneratorViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public bool testMode;
        public string endpoint = "voice-generator";
    }
}