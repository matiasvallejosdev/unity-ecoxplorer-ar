using UnityEngine;
using TMPro;
using R3;
using ViewModel;
using DanielLochner.Assets.SimpleScrollSnap;

namespace Components
{
    public class MainMenuTopicDisplay : MonoBehaviour
    {
        public MenuManageViewModel menuManageViewModel;
        public MainMenuTopicInput topicInput;
        public TextMeshProUGUI textComponent;
        public TextMeshProUGUI chapterTextComponent;

        [Header("Pulse Animation Settings")]
        [SerializeField] private float pulseMinScale = 0.95f;
        [SerializeField] private float pulseMaxScale = 1.05f;
        [SerializeField] private float pulseSpeed = 3f;

        private SimpleScrollSnap scrollSnap;
        private int myIndex;
        private bool isCentered;
        private float pulseTimer;
        private Vector3 baseScale;

        private const string chapterPrefixEs = "Capítulo ";
        private const string chapterPrefixEn = "Chapter ";

        public void Start()
        {
            topicInput.GetTopic().Subscribe(OnTopicChanged);
            menuManageViewModel.languageSelected.Subscribe(OnLanguageChanged);

            baseScale = transform.localScale;

            scrollSnap = GetComponentInParent<SimpleScrollSnap>();
            if (scrollSnap != null)
            {
                for (int i = 0; i < scrollSnap.Panels.Length; i++)
                {
                    if (scrollSnap.Panels[i].gameObject == gameObject)
                    {
                        myIndex = i;
                        break;
                    }
                }

                scrollSnap.OnPanelCentered.AddListener(OnPanelCentered);
            }
        }

        private void Update()
        {
            if (isCentered)
            {
                pulseTimer += Time.deltaTime * pulseSpeed;

                // Calculate the current scale using a sine wave
                float scaleRange = (pulseMaxScale - pulseMinScale) * 0.5f;
                float scaleMidpoint = (pulseMaxScale + pulseMinScale) * 0.5f;
                float currentScale = scaleMidpoint + Mathf.Sin(pulseTimer) * scaleRange;

                transform.localScale = baseScale * currentScale;
            }
        }

        private void OnPanelCentered(int newCenteredPanel, int previousCenteredPanel)
        {
            isCentered = (newCenteredPanel == myIndex);
            
            if (!isCentered)
            {
                transform.localScale = baseScale; // Reset scale when not centered
            }
        }

        private void OnTopicChanged(TopicDataViewModel topic)
        {
            var language = menuManageViewModel.languageSelected.Value;
            var topicName = language == "es" ? topic.TopicNames["es"] : topic.TopicNames["en"];
            textComponent.text = topicName;
            chapterTextComponent.text = language == "es" ? chapterPrefixEs + topic.chapter_number : chapterPrefixEn + topic.chapter_number;
        }

        private void OnLanguageChanged(string language)
        {
            var topic = topicInput.GetTopic().Value;
            var topicName = language == "es" ? topic.TopicNames["es"] : topic.TopicNames["en"];
            textComponent.text = topicName;
        }

        private void OnDestroy()
        {
            if (scrollSnap != null)
            {
                scrollSnap.OnPanelCentered.RemoveListener(OnPanelCentered);
            }
        }
    }
}