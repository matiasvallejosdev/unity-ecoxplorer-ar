using UnityEngine;
using R3;
using ViewModel;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Contracts;

namespace Components
{
    public class MenuGuidelineMotionDisplay : MonoBehaviour
    {
        [Header("References")]
        public GameManagerViewModel gameManagerViewModel;
        public TextMeshProUGUI guidelineText;

        [Header("Audio Settings")]
        public AudioSource audioSourceNotification;
        public AudioSource audioSourceType;
        public AudioClip typeAudio;
        public AudioClip notificationAudio;

        [Header("Timing Settings")]
        [SerializeField] private float initialDelay = 1f;
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float audioInterval = 0.1f;

        private Coroutine currentTypeCoroutine;
        private bool isTyping = false;
        private float showTime;
        private Queue<string> guidelineQueue = new Queue<string>();

        private void Start()
        {
            guidelineText.text = "";
            showTime = gameManagerViewModel.timeLineGuideline;
            gameManagerViewModel.OnMessage.Subscribe(OnGuidelines).AddTo(this);
            gameManagerViewModel.isSessionStarted.Subscribe(OnSessionStartedChanged).AddTo(this);
            gameManagerViewModel.isPlayingStory.Subscribe(OnPlayingStoryChanged).AddTo(this);
            gameManagerViewModel.isGameEnded.Subscribe(OnGameEnd).AddTo(this);
        }

        private void OnPlayingStoryChanged(bool isPlaying)
        {
            if (!isPlaying)
            {
                HideGuideline();
            }
        }

        private void OnSessionStartedChanged(bool started)
        {
            if (started)
            {
                HideGuideline();
            }
        }

        private void OnGuidelines(MessageDto messageDto)
        {
            if (string.IsNullOrEmpty(messageDto.message)) return;
            guidelineText.color = messageDto.color;
            guidelineQueue.Enqueue(messageDto.message);
            if (!isTyping)
            {
                ProcessNextGuideline();
            }
        }

        private void OnGameEnd(bool isGameEnded)
        {
            if (!isGameEnded) return;
            StopAllCoroutines();
            guidelineText.text = "";
        }

        private void ProcessNextGuideline()
        {
            if (guidelineQueue.Count > 0 && !isTyping)
            {
                string nextGuideline = guidelineQueue.Dequeue();
                StartTypingEffect(nextGuideline);
            }
            else
            {
                Debug.Log($"Queue is empty or still typing | Queue Count: {guidelineQueue.Count} | isTyping: {isTyping}");
            }
        }

        private void StartTypingEffect(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ProcessNextGuideline();
                return;
            }

            HideGuideline();
            currentTypeCoroutine = StartCoroutine(TypeText(text));
        }

        private void HideGuideline()
        {
            if (currentTypeCoroutine != null)
            {
                StopCoroutine(currentTypeCoroutine);
                currentTypeCoroutine = null;
            }

            isTyping = false;
            guidelineText.text = "";
        }

        private IEnumerator TypeText(string text)
        {
            isTyping = true;

            // Initial delay and notification sound
            yield return new WaitForSeconds(initialDelay);
            PlayNotificationSound();

            yield return new WaitForSeconds(0.5f);

            float lastAudioTime = 0;
            guidelineText.text = "";

            // Type each character
            foreach (char letter in text)
            {
                if (!isTyping) yield break;

                guidelineText.text += letter;

                // Play typing sound with interval control
                if (Time.time - lastAudioTime >= audioInterval)
                {
                    PlayTypeSound();
                    lastAudioTime = Time.time;
                }

                yield return new WaitForSeconds(typingSpeed);
            }

            // Wait and then hide
            yield return new WaitForSeconds(showTime);
            HideGuideline();
            isTyping = false;

            // Process next guideline in queue if any
            ProcessNextGuideline();
        }

        private void PlayNotificationSound()
        {
            if (audioSourceNotification != null && notificationAudio != null)
            {
                audioSourceNotification.clip = notificationAudio;
                audioSourceNotification.Play();
            }
        }

        private void PlayTypeSound()
        {
            if (audioSourceType != null && typeAudio != null)
            {
                audioSourceType.clip = typeAudio;
                audioSourceType.Play();
            }
        }

        private void OnDisable()
        {
            HideGuideline();
            guidelineQueue.Clear();
        }
    }
}