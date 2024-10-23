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
            gameManagerViewModel.isScanning.Subscribe(OnScanningChanged).AddTo(this);
        }

        private void OnScanningChanged(bool isActive)
        {
            rawImage.enabled = isActive;
            animator.SetBool("Scanning", isActive);
        }
    }
}