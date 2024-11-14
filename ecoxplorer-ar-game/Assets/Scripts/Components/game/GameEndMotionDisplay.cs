
using UnityEngine;
using ViewModel;
using R3;
namespace Components.Game
{
    public class GameEndMotionDisplay : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public GameCmdFactory gameCmdFactory;

        void Start()
        {
            gameManagerViewModel.OnGameEnd.Subscribe(OnGameEnd).AddTo(this);
        }

        private void OnGameEnd(bool value)
        {
            Debug.Log("Game ended");
        }
    }
}
