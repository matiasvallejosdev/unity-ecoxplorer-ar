

using System.Linq;
using Contracts;
using Newtonsoft.Json;
using UnityEngine;
using ViewModel;

namespace Commands
{
    public class SessionStartActionCmd : ICommand
    {
        private SessionStartDto _sessionStartDto;
        public SessionStartActionCmd(SessionStartDto sessionStartDto)
        {
            _sessionStartDto = sessionStartDto;
        }

        public async void Execute()
        {
            var topic = _sessionStartDto.menuManageViewModel.currentTopic.Value;
            if (topic == null)
            {
                Debug.LogError($"SessionStartActionCmd failed: topic is null");
                return;
            }
            Debug.Log("SessionStartActionCmd. Iniciando sesion con el topic: " + topic.TopicNames["es"]);
            _sessionStartDto.gameManagerViewModel.languageSelected = _sessionStartDto.menuManageViewModel.languageSelected;
            _sessionStartDto.gameManagerViewModel.currentTopicData = topic;
            var language = _sessionStartDto.menuManageViewModel.languageSelected.Value;
            var levelBuilderResponse = await _sessionStartDto.levelBuilderGateway.GetLevelBuilder(_sessionStartDto.levelBuilderServiceViewModel, topic.keywords, language);
            var levelDataViewModel = ScriptableObject.CreateInstance<LevelDataViewModel>();
            levelDataViewModel.levelTitle = levelBuilderResponse.Value.title;
            levelDataViewModel.levelStoryIntroduction = levelBuilderResponse.Value.story_introduction;
            levelDataViewModel.levelStoryEnd = levelBuilderResponse.Value.story_end;
            levelDataViewModel.levelGuidance = levelBuilderResponse.Value.guidance.ToList();
            levelDataViewModel.narratorName = levelBuilderResponse.Value.narrator;
            levelDataViewModel.primarySubject = levelBuilderResponse.Value.primary_subject;
            levelDataViewModel.levelGuidance = levelBuilderResponse.Value.guidance.ToList();
            _sessionStartDto.gameManagerViewModel.currentLevelData = levelDataViewModel;
            Debug.Log("SessionStartActionCmd. Level data: " + JsonConvert.SerializeObject(_sessionStartDto.gameManagerViewModel.currentLevelData));
            _sessionStartDto.gameManagerViewModel.currentStoryState = CurrentStoryState.Start;
            _sessionStartDto.gameManagerViewModel.currentScene = GameScene.Transition;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)GameScene.Transition);
        }
    }
}