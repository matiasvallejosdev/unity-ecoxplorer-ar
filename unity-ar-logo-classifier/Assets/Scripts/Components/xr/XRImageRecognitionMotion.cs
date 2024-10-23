using UnityEngine;
using ViewModel;
using R3;

namespace Components
{
    public class XRImageRecognitionMotion : MonoBehaviour 
    {
        public GameManagerViewModel recognitionManager;
        public AudioSource audioSource;

        void Start(){
            // recognitionManager.OnRecognitionStart.Subscribe(isActive => {
            //     if (isActive)
            //     {
            //         audioSource.Play();
            //     } else {
            //         audioSource.Stop();
            //     }
            // }).AddTo(this);
        }
    }
}