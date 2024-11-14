using System.Collections;
using System.Collections.Generic;
using Contracts;
using DanielLochner.Assets.SimpleScrollSnap;
using R3;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MainMenuTopicInput : MonoBehaviour
    {
        public GameManagerViewModel gameManager;
        public MenuManageViewModel menuManageViewModel;
        public MenuCmdFactory menuCmdFactory;
        public LevelBuilderServiceViewModel levelBuilderServiceViewModel;
        private ReactiveProperty<TopicDataViewModel> topic = new();

        public void SetTopic(TopicDataViewModel topic)
        {
            this.topic.Value = topic;
        }

        public ReactiveProperty<TopicDataViewModel> GetTopic()
        {
            return topic;
        }

        public void OnClick()
        {
            if (menuManageViewModel.isLoading.Value)
                return;
            menuManageViewModel.currentTopic.Value = topic.Value;
            var sessionStartDto = new SessionStartDto(){
                gameManagerViewModel = gameManager,
                menuManageViewModel = menuManageViewModel,
                levelBuilderServiceViewModel = levelBuilderServiceViewModel,
            };
            menuCmdFactory.MainMenuSessionStartCmd(sessionStartDto).Execute();
        }
    }
}
