using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Player Stories", menuName = "Player/Player Stories")]
    public class PlayerStoriesViewModel : ScriptableObject
    {
        public string story_id;
        public string image_id;
        public AudioClip audioClip;
        public StorytellerGptResponse story;
        public EnvironmentalImageAnalysisResponse imageAnalysis;
        public Texture2D image;
        internal string image_url;
    }
}
