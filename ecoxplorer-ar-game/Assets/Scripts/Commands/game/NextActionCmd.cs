
using Contracts;
using UnityEngine;
using ViewModel;

namespace Commands
{
    public class NextActionCmd : ICommand
    {
        private GameManagerViewModel _gameManagerViewModel;

        public NextActionCmd(GameManagerViewModel gameManagerViewModel)
        {
            _gameManagerViewModel = gameManagerViewModel;
        }

        public void Execute()
        {
            Debug.Log("NextActionCmd executed");
            _gameManagerViewModel.currentStoryState = CurrentStoryState.End;
            _gameManagerViewModel.currentScene = GameScene.Transition;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)GameScene.Transition);
        }
    }
}