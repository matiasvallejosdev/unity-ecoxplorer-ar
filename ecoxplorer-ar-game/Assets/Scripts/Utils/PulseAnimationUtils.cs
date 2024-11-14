using UnityEngine;
using System.Collections;

namespace Utils
{
    public static class PulseAnimationUtils
    {
        public static IEnumerator PulseScale(Transform target, Vector3 originalScale, float maxScale = 1.2f, float duration = 0.3f)
        {
            // Scale up
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(1f, maxScale, elapsedTime / duration);
                target.localScale = originalScale * scale;
                yield return null;
            }

            // Scale back
            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(maxScale, 1f, elapsedTime / duration);
                target.localScale = originalScale * scale;
                yield return null;
            }
        }
    }
} 