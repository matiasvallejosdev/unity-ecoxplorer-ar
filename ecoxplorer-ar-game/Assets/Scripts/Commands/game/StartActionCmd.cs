using UnityEngine;
using ViewModel;
using System.Threading.Tasks;
using System.Linq;
using Contracts;
using System;

namespace Commands
{
    public class StartActionCmd : ICommand
    {
        private GameManagerViewModel _gameManagerViewModel;

        public StartActionCmd(GameManagerViewModel gameManagerViewModel)
        {
            _gameManagerViewModel = gameManagerViewModel;
        }

        public void Execute()
        {
            Debug.Log("Game started. Showing guidelines.");
            StartGame();
        }

        private async void StartGame()
        {
            // Reset all game states first
            _gameManagerViewModel.actionIndications.Value = "...";
            _gameManagerViewModel.isSessionStarted.Value = false;
            _gameManagerViewModel.isLoadingPlay.Value = true;
            _gameManagerViewModel.isPlayingStory.Value = false;
            _gameManagerViewModel.isGameEnded.Value = false;
            _gameManagerViewModel.playerStories.Clear();
            _gameManagerViewModel.currentPlayerStory = null;
            _gameManagerViewModel.isGameEnded.Value = false;
            _gameManagerViewModel.storiesCompleted.Value = 0;
            _gameManagerViewModel.OnGameStart.OnNext(true);

            // Send guidelines in sequence - queue will handle timing
            await Task.Delay(400);
            _gameManagerViewModel.OnMessage.OnNext(new MessageDto(_gameManagerViewModel.startInstruction, Color.yellow));
            var guideline = _gameManagerViewModel.currentLevelData.levelGuidance.FirstOrDefault();
            _gameManagerViewModel.OnMessage.OnNext(new MessageDto(guideline, Color.yellow));

            var delay = _gameManagerViewModel.timeBetweenStories;
            await Task.Delay(TimeSpan.FromSeconds(delay));
            _gameManagerViewModel.actionIndications.Value = _gameManagerViewModel.pressToPlayInstruction;
            _gameManagerViewModel.isLoadingPlay.Value = false;
        }
    }
}
