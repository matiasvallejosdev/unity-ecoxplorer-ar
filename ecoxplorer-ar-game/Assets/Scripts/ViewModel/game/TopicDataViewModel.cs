using System.Collections.Generic;
using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Topic Data", menuName = "Game/Topic Data")]
    public class TopicDataViewModel : ScriptableObject
    {
        public int topic_id;
        public int chapter_number;
        [SerializeField] private string topicNameEs;
        [SerializeField] private string topicNameEn;

        private Dictionary<string, string> topic_names = new Dictionary<string, string>();

        private void OnEnable()
        {
            topic_names["es"] = topicNameEs;
            topic_names["en"] = topicNameEn;
        }

        public Dictionary<string, string> TopicNames => topic_names;
        public string topic_description;
        public string keywords;
        public string url = "no-url";

        public string GetTopicName(string language)
        {
            return topic_names[language];
        }
    }
}
