
using UnityEngine;
using ViewModel;

namespace Components
{
    public class TransitionNextButtonInput : MonoBehaviour
    {
        public GameCmdFactory gameCmdFactory;
        public GameManagerViewModel gameManagerViewModel;

        public void OnClick()
        {
            gameCmdFactory.TransitionNextActionCmd(gameManagerViewModel).Execute();
        }
    }
}
