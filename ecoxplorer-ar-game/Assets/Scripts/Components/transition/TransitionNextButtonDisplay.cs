using UnityEngine;
using ViewModel;
using System.Collections;
using UnityEngine.UI;

namespace Components
{
    public class TransitionNextButtonDisplay : MonoBehaviour
    {
        [Header("Configuration")]
        public float pulseDuration = 0.3f;
        public float maxScale = 1.2f;
        public float pauseBetweenPulses = 0.1f;
        public int pulseCount = 0;  // 0 means infinite pulses
        
        public GameCmdFactory gameCmdFactory;
        public GameManagerViewModel gameManagerViewModel;
        private Vector3 originalScale;

        void Start()
        {
            originalScale = transform.localScale;
            StartCoroutine(PulseButton());
        }

        private IEnumerator PulseButton()
        {
            int currentPulse = 0;
            while (pulseCount == 0 || currentPulse < pulseCount)
            {
                // Scale up
                float elapsedTime = 0;
                while (elapsedTime < pulseDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float scale = Mathf.Lerp(1f, maxScale, elapsedTime / pulseDuration);
                    transform.localScale = originalScale * scale;
                    yield return null;
                }

                // Scale back
                elapsedTime = 0;
                while (elapsedTime < pulseDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float scale = Mathf.Lerp(maxScale, 1f, elapsedTime / pulseDuration);
                    transform.localScale = originalScale * scale;
                    yield return null;
                }

                if (pulseCount > 0) currentPulse++;
                if (currentPulse < pulseCount || pulseCount == 0)
                    yield return new WaitForSeconds(pauseBetweenPulses);
            }
        }
    }
}
