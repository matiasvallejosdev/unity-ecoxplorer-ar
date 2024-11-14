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
        [SerializeField] private float wordsPerMinute = 150f;
        [SerializeField] private float pauseAfterPeriod = 0.3f;
        [SerializeField] private float pauseAfterComma = 0.15f;
        [SerializeField] private float fadeInDuration = 0.05f;
        [SerializeField] private float fadeOutDuration = 0.05f;
        [SerializeField] private int maxCharsPerLine = 50;
        [SerializeField] private float maxSegmentDuration = 3f;

        [Header("Timing Adjustments")]
        [SerializeField] private float subtitleOffset = -0.1f;
        [SerializeField] private float timingTolerance = 0.05f;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;

        private List<SubtitleSegment> currentSegments;
        private IDisposable updateSubscription;
        private bool isPlaying;
        private Subject<Unit> onSubtitleEnd = new Subject<Unit>();

        void Start()
        {
            if (gameManagerViewModel == null)
            {
                Debug.LogError("GameManagerViewModel is not assigned!");
                return;
            }

            // Subscribe to story start
            gameManagerViewModel.OnStoryStart
                .Subscribe(OnPlayStory)
                .AddTo(this);

            // Subscribe to story finish
            gameManagerViewModel.OnStoryEnd
                .Subscribe(_ => OnStoryFinish())
                .AddTo(this);

            ResetSubtitleState();
        }

        private void ResetSubtitleState()
        {
            if (subtitleLabel != null)
            {
                subtitleLabel.text = string.Empty;
                subtitleLabel.alpha = 0;
            }
            isPlaying = false;
            currentSegments = null;
            updateSubscription?.Dispose();
            updateSubscription = null;
        }

        private void OnStoryFinish()
        {
            StopSubtitles();
        }

        void OnPlayStory(PlayerStoriesViewModel playerStory)
        {
            if (playerStory == null || string.IsNullOrEmpty(playerStory.story?.story))
            {
                Debug.LogError("Invalid story data received");
                return;
            }

            StopSubtitles();
            currentSegments = GenerateSubtitleSegments(playerStory.story.story, playerStory.audioClip.length);
            StartSubtitles();
        }

        private void StartSubtitles()
        {
            if (currentSegments == null || currentSegments.Count == 0 || audioSource == null) 
            {
                Debug.LogWarning("Cannot start subtitles: missing segments or audio source");
                return;
            }

            isPlaying = true;

            updateSubscription = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (!isPlaying || audioSource == null) return;
                    
                    if (audioSource.isPlaying)
                    {
                        float adjustedTime = audioSource.time + subtitleOffset;
                        UpdateSubtitles(adjustedTime);
                    }
                    else if (audioSource.time >= audioSource.clip.length)
                    {
                        StopSubtitles();
                    }
                })
                .AddTo(this);
        }

        private void UpdateSubtitles(float currentTime)
        {
            if (currentSegments == null || !isPlaying) return;

            var currentSegment = currentSegments
                .FirstOrDefault(s => 
                    currentTime >= (s.startTime - timingTolerance) && 
                    currentTime < (s.startTime + s.duration + timingTolerance));

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
                else if (segmentProgress > 1f - fadeOutDuration)
                {
                    alpha = (1f - segmentProgress) / fadeOutDuration;
                }

                subtitleLabel.alpha = Mathf.Clamp01(alpha);
            }
            else
            {
                if (!string.IsNullOrEmpty(subtitleLabel.text))
                {
                    subtitleLabel.text = string.Empty;
                    subtitleLabel.alpha = 0;
                }
            }
        }

        private void StopSubtitles()
        {
            isPlaying = false;
            updateSubscription?.Dispose();
            updateSubscription = null;
            ResetSubtitleState();
        }

        private List<SubtitleSegment> GenerateSubtitleSegments(string storyText, float audioDuration)
        {
            var segments = new List<SubtitleSegment>();
            float baseTimePerWord = 60f / wordsPerMinute;
            float currentTime = 0f;

            var sentences = Regex.Split(storyText, @"(?<=[.!?])\s+")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim());

            foreach (var sentence in sentences)
            {
                var chunks = Regex.Split(sentence, @"((?<=[,;:])\s+)")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim());

                foreach (var chunk in chunks)
                {
                    var words = chunk.Split(' ');
                    float chunkDuration = words.Length * baseTimePerWord;

                    if (chunk.EndsWith(".") || chunk.EndsWith("!") || chunk.EndsWith("?"))
                    {
                        chunkDuration += pauseAfterPeriod;
                    }
                    else if (chunk.EndsWith(",") || chunk.EndsWith(";") || chunk.EndsWith(":"))
                    {
                        chunkDuration += pauseAfterComma;
                    }

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

        private void OnDestroy()
        {
            StopSubtitles();
            onSubtitleEnd?.Dispose();
        }
    }
}