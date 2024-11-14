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
        public Texture2D icon;

        void Start()
        {
            gameManagerViewModel.storiesCompleted.Subscribe(OnStoriesCompleted).AddTo(this);
        }
        public void OnStoriesCompleted(int storiesCompleted)
        {
            if (storiesCompleted <= 0 || storiesCompleted > slots.Length)
            {
                return;
            }

            for (int i = 0; i < storiesCompleted && i < slots.Length; i++)
            {
                Color color = Color.white;
                color.a = 1f; // Set alpha to 1 for full opacity
                slots[i].color = color;
                slots[i].texture = icon;
            }

            audioSource.Play();
        }
    }
}