using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MainMenuTopicMotionDisplay : MonoBehaviour
    {
        public SimpleScrollSnap scrollSnap;
        public AudioSource audioSource;
        public AudioClip clip;

        private void Start()
        {
            scrollSnap.OnPanelCentered.AddListener(OnPanelCentered);
        }

        private void OnPanelCentered(int currentPanel, int previousPanel)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
