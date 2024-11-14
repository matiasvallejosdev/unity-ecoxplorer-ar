using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "LevelBuilderService", menuName = "Services/Level Builder Service", order = 0)]
    public class LevelBuilderServiceViewModel : ScriptableObject 
    {
        public ApiServiceViewModel apiService; // Your API Service
        public bool testMode;
        public string endpoint = "level-builder";
    }
}