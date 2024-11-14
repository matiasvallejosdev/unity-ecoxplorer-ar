using UnityEngine;
using ViewModel;
using System.Linq;
using Contracts;

namespace Infrastructure
{
    public class StoryFinishActionCmd : ICommand
    {
        private GameManagerViewModel _gameManagerViewModel;

        public StoryFinishActionCmd(GameManagerViewModel gameManagerViewModel)
        {
            _gameManagerViewModel = gameManagerViewModel;
        }

        public void Execute()
        {
            Debug.Log("Story was finished");
            if (!_gameManagerViewModel.isGameEnded.Value)
            {
                _gameManagerViewModel.OnStoryEnd.OnNext(true);
                _gameManagerViewModel.storiesCompleted.Value++;
                var pressToPlay = _gameManagerViewModel.pressToPlayInstruction;
                _gameManagerViewModel.actionIndications.Value = pressToPlay;
                _gameManagerViewModel.isSessionStarted.Value = false;
                _gameManagerViewModel.isLoadingPlay.Value = false;
                _gameManagerViewModel.isPlayingStory.Value = false;
            }
            if (_gameManagerViewModel.currentPlayerStory != null)
            {
                var callToAction = _gameManagerViewModel.currentPlayerStory.story.call_to_action;
                _gameManagerViewModel.OnMessage.OnNext(new MessageDto(callToAction.suggestion, Color.green));
            }
        }
    }
}
