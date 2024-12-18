using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Player Stories", menuName = "Player/Player Stories")]
    public class PlayerStoriesViewModel : ScriptableObject
    {
        public string story_id;
        public StorytellerGptResponse story;
        public AudioClip audioClip;
        public EnvironmentalImageAnalysisResponse imageAnalysis;
        public Texture2D image;
        public string image_id;
        public string image_url;
    }
}
