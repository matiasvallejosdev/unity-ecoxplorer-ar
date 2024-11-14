using System.Collections;
using System.Collections.Generic;
using Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

public class TransitionStoryDisplay : MonoBehaviour
{
    [Header("References")]
    public GameManagerViewModel gameManagerViewModel;
    public TextMeshProUGUI label;
    public Button nextButton;

    [Header("Audio Settings")]
    public AudioSource notificationAudioSource;
    public AudioClip notificationSound;
    public AudioClip winMelodySound;
    public AudioSource audioSource;
    public AudioClip typeSound;

    [Header("Timing Settings")]
    [SerializeField] private float initialDelay = 0.8f;
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float audioInterval = 0.08f;
    [SerializeField] private float delayBetweenTitleAndBody = 1.5f;
    [SerializeField] private float delayBetweenBodyAndManual = 1.5f;

    [Header("Color Settings")]
    [SerializeField] private Color titleColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color bodyColor = Color.white;
    [SerializeField] private Color manualColor = new Color(0.8f, 0.8f, 0.2f);

    private LevelDataViewModel _currentLevelData;
    private string _title;
    private string _body;
    private string[] _manual;
    private Coroutine currentTypeCoroutine;
    private bool isTyping = false;

    void Start()
    {
        _currentLevelData = gameManagerViewModel.currentLevelData;
        var isStart = gameManagerViewModel.currentStoryState == CurrentStoryState.Start;
        
        if (isStart)
        {
            _title = _currentLevelData.levelTitle;
            _body = _currentLevelData.levelStoryIntroduction;
            _manual = _currentLevelData.levelGuidance.ToArray();
            
            if (notificationAudioSource != null && notificationSound != null)
            {
                notificationAudioSource.clip = notificationSound;
                notificationAudioSource.Play();
            }
        }
        else
        {
            _title = "¡Felicitaciones!";
            _body = _currentLevelData.levelStoryEnd;
            _manual = null;
            
            if (notificationAudioSource != null && winMelodySound != null)
            {
                notificationAudioSource.clip = winMelodySound;
                notificationAudioSource.Play();
            }
        }

        // Disable button at start
        nextButton.interactable = false;
        
        StartSequentialDisplay();
    }

    private void StartSequentialDisplay()
    {
        StartCoroutine(SequentialTypeRoutine());
    }

    private IEnumerator SequentialTypeRoutine()
    {
        // First display title with yellow color and centered alignment
        label.color = titleColor;
        label.alignment = TextAlignmentOptions.Center;
        yield return StartCoroutine(TypeText(_title));
        
        // Wait between title and body
        yield return new WaitForSeconds(delayBetweenTitleAndBody);
        
        // Then display body with white color and left alignment
        label.color = bodyColor;
        label.alignment = TextAlignmentOptions.Left;
        yield return StartCoroutine(TypeText(_body));

        // If we have a manual (only in start state), display it
        if (_manual != null)
        {
            // Wait between body and manual
            yield return new WaitForSeconds(delayBetweenBodyAndManual);
            
            // Display manual with yellow color, keeping left alignment
            label.color = manualColor;
            label.alignment = TextAlignmentOptions.Left;
            
            // Create bulleted guidelines as a single string
            string bulletedGuidelines = "";
            for (int i = 0; i < _manual.Length; i++)
            {
                bulletedGuidelines += $"- {_manual[i]}\n";
            }
            
            yield return StartCoroutine(TypeText(bulletedGuidelines));
        }

        // Enable button after everything is typed
        nextButton.interactable = true;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        label.text = "";
        
        yield return new WaitForSeconds(initialDelay);

        float lastAudioTime = 0;

        foreach (char letter in text)
        {
            if (!isTyping) yield break;

            label.text += letter;

            if (Time.time - lastAudioTime >= audioInterval)
            {
                PlayTypeSound();
                lastAudioTime = Time.time;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void PlayTypeSound()
    {
        if (audioSource != null && typeSound != null)
        {
            audioSource.clip = typeSound;
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        if (currentTypeCoroutine != null)
        {
            StopCoroutine(currentTypeCoroutine);
        }
        isTyping = false;
        nextButton.interactable = false;
    }
}
