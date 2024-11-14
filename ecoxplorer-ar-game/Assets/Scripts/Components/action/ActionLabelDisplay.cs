using ViewModel;
using R3;
using UnityEngine;
using TMPro;
using System.Collections;

public class ActionLabelDisplay : MonoBehaviour
{
    public GameManagerViewModel gameManagerViewModel;
    public TextMeshProUGUI actionLabel;
    
    [Header("Pulse Effect Settings")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    
    [Header("Typing Effect Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    
    private Coroutine pulseCoroutine;
    private Coroutine typingCoroutine;

    public void Start()
    {
        gameManagerViewModel.actionIndications.Subscribe(indication => OnIndication(indication)).AddTo(this);
        gameManagerViewModel.isGameEnded.Subscribe(isGameEnded => OnGameEnd(isGameEnded)).AddTo(this);
    }

    private void OnGameEnd(bool isGameEnded)
    {
        if (!isGameEnded) return;
        StopAllCoroutines();
        actionLabel.text = "...";
    }

    private void OnIndication(string indication)
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        actionLabel.text = indication;
        typingCoroutine = StartCoroutine(ScaleInText());
        pulseCoroutine = StartCoroutine(PulseText());
    }

    private IEnumerator ScaleInText()
    {
        actionLabel.transform.localScale = Vector3.zero;
        float elapsedTime = 0f;
        
        while (elapsedTime < typingSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / typingSpeed;
            actionLabel.transform.localScale = Vector3.one * Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }
        
        actionLabel.transform.localScale = Vector3.one;
        typingCoroutine = null;
    }

    private IEnumerator PulseText()
    {
        while (true)
        {
            // Pulse out
            for (float t = 0; t <= 1; t += Time.deltaTime * pulseSpeed)
            {
                float scale = Mathf.Lerp(minScale, maxScale, t);
                actionLabel.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            // Pulse in
            for (float t = 0; t <= 1; t += Time.deltaTime * pulseSpeed)
            {
                float scale = Mathf.Lerp(maxScale, minScale, t);
                actionLabel.transform.localScale = Vector3.one * scale;
                yield return null;
            }
        }
    }
}
