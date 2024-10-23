using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Components
{
    public class StorySubtitleController : MonoBehaviour
    {
        [Header("UI References")]
        public RectTransform subtitlePanel;
        public TextMeshProUGUI subtitleText;

        [Header("Style Settings")]
        [SerializeField] private float paddingHorizontal = 40f;
        [SerializeField] private float paddingVertical = 20f;
        [SerializeField] private float bottomOffset = 50f;

        [Header("Background")]
        public Image backgroundImage;
        [SerializeField] private float backgroundAlpha = 0.8f;

        void Start()
        {
            SetupSubtitleUI();
        }

        private void SetupSubtitleUI()
        {
            // Position the subtitle panel
            if (subtitlePanel != null)
            {
                subtitlePanel.anchorMin = new Vector2(0, 0);
                subtitlePanel.anchorMax = new Vector2(1, 0);
                subtitlePanel.pivot = new Vector2(0.5f, 0);
                subtitlePanel.anchoredPosition = new Vector2(0, bottomOffset);
            }

            // Setup the text component
            if (subtitleText != null)
            {
                subtitleText.alignment = TextAlignmentOptions.Center;
                subtitleText.enableWordWrapping = true;
                subtitleText.margin = new Vector4(paddingHorizontal, paddingVertical,
                                                paddingHorizontal, paddingVertical);
            }

            // Setup the background
            if (backgroundImage != null)
            {
                var color = backgroundImage.color;
                color.a = backgroundAlpha;
                backgroundImage.color = color;
            }
        }

        public void UpdatePanelSize()
        {
            if (subtitlePanel != null && subtitleText != null)
            {
                // Update panel size based on text content
                var textBounds = subtitleText.textBounds;
                subtitlePanel.sizeDelta = new Vector2(
                    textBounds.size.x + paddingHorizontal * 2,
                    textBounds.size.y + paddingVertical * 2
                );
            }
        }
    }
}