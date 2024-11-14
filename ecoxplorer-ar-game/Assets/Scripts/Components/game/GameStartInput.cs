using UnityEngine;
using ViewModel;

namespace Components
{
    public class GameStartInput : MonoBehaviour
    {
        public GameManagerViewModel gameManagerViewModel;
        public GameCmdFactory gameCmdFactory;

        void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            gameCmdFactory.StartActionCmd(gameManagerViewModel).Execute();
        }
    }
}
