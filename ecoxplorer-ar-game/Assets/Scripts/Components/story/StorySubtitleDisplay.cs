using UnityEngine;
using ViewModel;
using R3;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Components
{
    [Serializable]
    public class SubtitleSegment
    {
        public string text;
        public float startTime;
        public float duration;
    }

    public class StorySubtitleDisplay : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI subtitleLabel;
        public AudioSource audioSource;

        [Header("View Model")]
        public GameManagerViewModel gameManagerViewModel;

        [Header("OpenAI TTS Settings")]
        [SerializeField] private float wordsPerMinute = 155f; // OpenAI TTS average speed
        [SerializeField] private float pauseAfterPeriod = 0.35f; // Pause after sentence
        [SerializeField] private float pauseAfterComma = 0.15f; // Pause after comma
        [SerializeField] private float fadeInDuration = 0.15f;
        [SerializeField] private float fadeOutDuration = 0.15f;
        [SerializeField] private int maxCharsPerLine = 50;
        [SerializeField] private float maxSegmentDuration = 3f;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;

        private List<SubtitleSegment> currentSegments;
        private IDisposable updateSubscription;
        private bool isPlaying;
        private Subject<Unit> onSubtitleEnd = new Subject<Unit>();

        void Start()
        {
            gameManagerViewModel.OnPlayStory
                .Subscribe(OnPlayStory)
                .AddTo(this);

            subtitleLabel.text = string.Empty;
            subtitleLabel.alpha = 0;
        }

        void OnPlayStory(PlayerStoriesViewModel playerStory)
        {
            StopSubtitles();
            currentSegments = GenerateSubtitleSegments(playerStory.story.story, playerStory.audioClip.length);
            StartSubtitles(playerStory);
        }

        private List<SubtitleSegment> GenerateSubtitleSegments(string storyText, float audioDuration)
        {
            var segments = new List<SubtitleSegment>();
            float baseTimePerWord = 60f / wordsPerMinute; // Time per word in seconds

            // Split into sentences first
            var sentences = Regex.Split(storyText, @"(?<=[.!?])\s+")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim());

            float currentTime = 0f;

            foreach (var sentence in sentences)
            {
                // Split sentence into chunks that include punctuation
                var chunks = Regex.Split(sentence, @"((?<=[,;:])\s+)")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim());

                foreach (var chunk in chunks)
                {
                    var words = chunk.Split(' ');
                    float chunkDuration = words.Length * baseTimePerWord;

                    // Add pauses for punctuation
                    if (chunk.EndsWith(".") || chunk.EndsWith("!") || chunk.EndsWith("?"))
                    {
                        chunkDuration += pauseAfterPeriod;
                    }
                    else if (chunk.EndsWith(",") || chunk.EndsWith(";") || chunk.EndsWith(":"))
                    {
                        chunkDuration += pauseAfterComma;
                    }

                    // Split long chunks into smaller segments
                    if (chunk.Length > maxCharsPerLine || chunkDuration > maxSegmentDuration)
                    {
                        var subChunks = SplitIntoLines(chunk, maxCharsPerLine);
                        foreach (var subChunk in subChunks)
                        {
                            var subWords = subChunk.Split(' ');
                            float subDuration = subWords.Length * baseTimePerWord;

                            segments.Add(new SubtitleSegment
                            {
                                text = subChunk,
                                startTime = currentTime,
                                duration = subDuration
                            });

                            currentTime += subDuration;
                        }
                    }
                    else
                    {
                        segments.Add(new SubtitleSegment
                        {
                            text = chunk,
                            startTime = currentTime,
                            duration = chunkDuration
                        });

                        currentTime += chunkDuration;
                    }
                }
            }

            // Normalize timings to match audio duration
            float totalDuration = segments.Sum(s => s.duration);
            float scaleFactor = audioDuration / totalDuration;

            foreach (var segment in segments)
            {
                segment.startTime *= scaleFactor;
                segment.duration *= scaleFactor;
            }

            if (showDebugInfo)
            {
                Debug.Log($"Generated {segments.Count} segments for {audioDuration:F2}s audio");
                foreach (var segment in segments)
                {
                    Debug.Log($"[{segment.startTime:F2}s - {segment.startTime + segment.duration:F2}s]: {segment.text}");
                }
            }

            return segments;
        }

        private List<string> SplitIntoLines(string text, int maxLength)
        {
            var lines = new List<string>();
            var words = text.Split(' ');
            var currentLine = new List<string>();
            int currentLength = 0;

            foreach (var word in words)
            {
                if (currentLength + word.Length + 1 > maxLength && currentLine.Count > 0)
                {
                    lines.Add(string.Join(" ", currentLine));
                    currentLine.Clear();
                    currentLength = 0;
                }

                currentLine.Add(word);
                currentLength += word.Length + 1;
            }

            if (currentLine.Count > 0)
            {
                lines.Add(string.Join(" ", currentLine));
            }

            return lines;
        }

        private void StartSubtitles(PlayerStoriesViewModel playerStory)
        {
            if (currentSegments == null || currentSegments.Count == 0) return;

            isPlaying = true;

            updateSubscription = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (isPlaying && audioSource != null)
                    {
                        UpdateSubtitles(audioSource.time);
                    }
                })
                .AddTo(this);
        }

        private void UpdateSubtitles(float currentTime)
        {
            var currentSegment = currentSegments
                .FirstOrDefault(s =>
                    currentTime >= s.startTime &&
                    currentTime < s.startTime + s.duration);

            if (currentSegment != null)
            {
                if (subtitleLabel.text != currentSegment.text)
                {
                    subtitleLabel.text = currentSegment.text;
                    if (showDebugInfo)
                    {
                        Debug.Log($"Showing subtitle at {currentTime:F2}s: {currentSegment.text}");
                    }
                }

                float segmentProgress = (currentTime - currentSegment.startTime) / currentSegment.duration;
                float alpha = 1f;

                if (segmentProgress < fadeInDuration)
                {
                    alpha = segmentProgress / fadeInDuration;
                }
                else if (segmentProgress > 1 - fadeOutDuration)
                {
                    alpha = (1 - segmentProgress) / fadeOutDuration;
                }

                subtitleLabel.alpha = Mathf.Clamp01(alpha);
            }
            else
            {
                subtitleLabel.text = string.Empty;
                subtitleLabel.alpha = 0;
            }
        }

        private void StopSubtitles()
        {
            isPlaying = false;
            updateSubscription?.Dispose();
            subtitleLabel.text = string.Empty;
            subtitleLabel.alpha = 0;
            currentSegments = null;
        }

        private void OnDestroy()
        {
            updateSubscription?.Dispose();
            onSubtitleEnd?.Dispose();
        }

        public void AdjustGlobalOffset(float offsetSeconds)
        {
            if (currentSegments == null) return;
            foreach (var segment in currentSegments)
            {
                segment.startTime += offsetSeconds;
            }
        }
    }
}