using UnityEngine;
using ViewModel;
using System.Threading.Tasks;
using Contracts;

namespace Commands
{
    public class EndActionCmd : ICommand
    {
        private GameManagerViewModel _gameManagerViewModel;

        public EndActionCmd(GameManagerViewModel gameManagerViewModel)
        {
            _gameManagerViewModel = gameManagerViewModel;
        }

        public async void Execute()
        {
            Debug.Log("Game ended. Good job.");
            _gameManagerViewModel.isGameEnded.Value = true;
            _gameManagerViewModel.OnGameEnd.OnNext(true);
            await Task.Delay(500);
            _gameManagerViewModel.actionIndications.Value = "...";
            _gameManagerViewModel.OnMessage.OnNext(new MessageDto(_gameManagerViewModel.endInstruction, Color.green));
        }
    }
}
