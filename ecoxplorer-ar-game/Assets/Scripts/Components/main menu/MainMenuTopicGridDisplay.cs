using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MainMenuTopicGridDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManager;
        public GameObject topicGrid;
        public GameObject topicPrefab;
        public SimpleScrollSnap scrollSnap;

        private void Start()
        {
            var topics = gameManager.topics;
            foreach (var topic in topics)
            {
                var topicObject = Instantiate(topicPrefab, topicGrid.transform);
                topicObject.GetComponent<MainMenuTopicInput>().SetTopic(topic);
                topicObject.name = topic.TopicNames["en"];
            }
            scrollSnap.GoToPanel(0);
        }
    }
}
