
using System.Collections;
using R3;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class StoryPlayInput : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public GameCmdFactory gameCmdFactory;

        void Start()
        {
            gameManagerViewModel.OnPlayStory.Subscribe(OnPlayStory);
        }

        private void OnPlayStory(PlayerStoriesViewModel story)
        {
            // Start the story
            gameCmdFactory.StoryCmd(gameManagerViewModel, story).Execute();

            // Finish the story after the audio clip length + 1 second
            var seconds = story.audioClip.length + 1.0f;
            StartCoroutine(FinishPlayStory(seconds));
        }

        private IEnumerator FinishPlayStory(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            gameCmdFactory.FinishPlayStoryCmd(gameManagerViewModel).Execute();
        }
    }
}
