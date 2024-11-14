
using System.Collections;
using R3;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class StoryPlayInput : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public StoryCmdFactory storyCmdFactory;

        void Start()
        {
            gameManagerViewModel.OnStoryStart.Subscribe(OnPlayStory);
        }

        private void OnPlayStory(PlayerStoriesViewModel story)
        {
            var seconds = story.audioClip.length + 1.0f;
            StartCoroutine(FinishPlayStory(seconds));
        }

        private IEnumerator FinishPlayStory(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            storyCmdFactory.StoryFinishCmd(gameManagerViewModel).Execute();
        }
    }
}
