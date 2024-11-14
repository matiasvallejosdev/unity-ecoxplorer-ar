using UnityEngine;
using R3;
using ViewModel;
using System;
using UnityEngine.UI;

namespace Components
{
    public class MenuTextureMotionDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public Animator animator;
        public RawImage rawImage;

        private void Start()
        {
            gameManagerViewModel.isSessionStarted.Subscribe(OnSessionStartedChanged).AddTo(this);
        }

        private void OnSessionStartedChanged(bool isPlaying)
        {
            rawImage.enabled = isPlaying;
            animator.SetBool("Scanning", isPlaying);
        }
    }
}