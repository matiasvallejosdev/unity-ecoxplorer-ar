
using Contracts;
using UnityEngine;
using ViewModel;

namespace Commands
{
    public class TransitionNextActionCmd : ICommand
    {
        private readonly GameManagerViewModel _gameManagerViewModel;

        public TransitionNextActionCmd(GameManagerViewModel gameManagerViewModel)
        {
            _gameManagerViewModel = gameManagerViewModel;
        }

        public void Execute()
        {
            var isStart = _gameManagerViewModel.currentStoryState == CurrentStoryState.Start;
            if (isStart)
            {
                Debug.Log("TransitionNextActionCmd: Start");
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)GameScene.Game);
            }
            else
            {
                Debug.Log("TransitionNextActionCmd: End");
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)GameScene.MainMenu);
            }
        }
    }
}
