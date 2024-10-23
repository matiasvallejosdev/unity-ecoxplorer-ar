using UnityEngine;
using ViewModel;
using System.Linq;

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
            _gameManagerViewModel.isLoadingPlay.Value = false;
            _gameManagerViewModel.isPlayingStory.Value = false;
            _gameManagerViewModel.OnFinishStory.OnNext(true);
            _gameManagerViewModel.storiesCompleted.Value++;
        }
    }
}
