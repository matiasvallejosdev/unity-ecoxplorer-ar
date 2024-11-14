using UnityEngine;
using ViewModel;
using R3;
namespace Components
{
    public class GameEndInput : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public GameCmdFactory gameCmdFactory;

        void Start()
        {
            gameManagerViewModel.storiesCompleted.Subscribe(OnStoriesCompleted).AddTo(this);    
        }

        private void OnStoriesCompleted(int storiesCompleted)
        {
            if (storiesCompleted >= gameManagerViewModel.storiesToComplete)
            {
                gameCmdFactory.EndActionCmd(gameManagerViewModel).Execute();
            }
        }
    }
}
