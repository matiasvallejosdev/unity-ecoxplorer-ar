
using R3;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class StoryAudioDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public AudioSource audioSource;

        void Start()
        {
            gameManagerViewModel.OnPlayStory.Subscribe(OnPlayStory);
        }

        private void OnPlayStory(PlayerStoriesViewModel story)
        {
            PlayAudio(story.audioClip);
        }

        public void PlayAudio(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void StopAudio()
        {
            audioSource.Stop();
        }

        void OnDisable()
        {
            StopAudio();
        }
    }
}
