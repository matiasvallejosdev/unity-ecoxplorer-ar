
using System.Collections;
using Contracts;
using R3;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class StoryLoadInput : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public StoryCmdFactory storyCmdFactory;
        public StorytellerViewModel storytellerViewModel;
        public RecognitionServiceViewModel recognitionService;
        public VoiceGeneratorViewModel voiceGeneratorViewModel;

        void Start()
        {
            gameManagerViewModel.OnStoryLoad.Subscribe(OnStoryLoad).AddTo(this);
        }

        private void OnStoryLoad(bool value)
        {
            var storyLoadCmdDto = new StoryLoadCmdDto(){
                gameManagerViewModel = gameManagerViewModel,
                storytellerViewModel = storytellerViewModel,
                recognitionService = recognitionService,
                voiceGeneratorViewModel = voiceGeneratorViewModel
            };
            storyCmdFactory.StoryLoadActionCmd(storyLoadCmdDto).Execute();
        }
    }
}
