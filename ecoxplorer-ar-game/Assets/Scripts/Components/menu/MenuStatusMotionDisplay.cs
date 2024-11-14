using UnityEngine;
using R3;
using ViewModel;
using UnityEngine.UI;
using Contracts;

namespace Components
{
    public class MenuStatusMotionDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public AudioSource audioSource;
        public GameObject status;
        public RawImage statusImage;
        public AudioClip goodAudio;
        public AudioClip badAudio;


        private void Start()
        {
            statusImage.enabled = false;
            gameManagerViewModel.OnMatch.Subscribe(OnMatchChanged).AddTo(this);
            gameManagerViewModel.isLoadingPlay.Subscribe(isLoading => status.SetActive(!isLoading)).AddTo(this);
            gameManagerViewModel.isPlayingStory.Subscribe(isPlaying => status.SetActive(!isPlaying)).AddTo(this);
        }

        private void OnMatchChanged(MatchReactiveDto matchReactiveDto)
        {
            if (matchReactiveDto.isCancelledOperation)
            {
                statusImage.enabled = false;
                audioSource.clip = badAudio;
                audioSource.Play();
                return;
            }

            if (matchReactiveDto.isMatch)
            {
                status.SetActive(false);
                status.SetActive(true);
                statusImage.enabled = true;
                matchReactiveDto.matchTexture.Apply();
                statusImage.texture = matchReactiveDto.matchTexture;
                audioSource.clip = goodAudio;
                audioSource.Play();
            }
            else
            {
                statusImage.enabled = false;
                audioSource.clip = badAudio;
                audioSource.Play();
            }
        }
    }
}