
using UnityEngine;
using R3;
using ViewModel;
using System.Collections;

namespace Components
{
    public class MenuNextDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public GameObject subtitleDisplay;
        public GameObject nextButton;
        private Vector3 originalScale;

        void Start()
        {
            gameManagerViewModel.isGameEnded.Subscribe(OnGameEnd).AddTo(this);
            nextButton.SetActive(false);
            subtitleDisplay.SetActive(true);
            originalScale = nextButton.transform.localScale;
        }

        public void OnGameEnd(bool isGameEnded)
        {
            nextButton.SetActive(isGameEnded);
            subtitleDisplay.SetActive(!isGameEnded);
            if (isGameEnded)
            {
                StartCoroutine(PulseButton());
            }
        }

        private IEnumerator PulseButton()
        {
            for (int i = 0; i < 2; i++)
            {
                // Scale up
                float duration = 0.3f;
                float elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float scale = Mathf.Lerp(1f, 1.2f, elapsedTime / duration);
                    nextButton.transform.localScale = originalScale * scale;
                    yield return null;
                }

                // Scale back
                elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float scale = Mathf.Lerp(1.2f, 1f, elapsedTime / duration);
                    nextButton.transform.localScale = originalScale * scale;
                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
