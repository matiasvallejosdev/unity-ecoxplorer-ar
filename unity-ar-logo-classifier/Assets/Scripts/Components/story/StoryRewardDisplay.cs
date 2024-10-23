using UnityEngine;
using ViewModel;
using R3;
using System.Collections;
using UnityEngine.UI;

namespace Components
{
    public class StoryRewardDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public RawImage[] slots;
        public AudioSource audioSource;

        void Start()
        {
            gameManagerViewModel.storiesCompleted.Subscribe(OnStoriesCompleted).AddTo(this);
        }

        public void OnStoriesCompleted(int storiesCompleted)
        {
            var index = storiesCompleted - 1;
            if (index < slots.Length)
            {
                Color color = slots[storiesCompleted].color;
                color.a = 1f; // Set alpha to 1 for full opacity
                slots[index].color = color;
                audioSource.Play();
            }
        }
    }
}