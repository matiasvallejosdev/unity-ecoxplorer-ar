
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components
{
    public class MenuNextInput : MonoBehaviour
    {
        public GameCmdFactory gameCmdFactory;
        public GameManagerViewModel gameManagerViewModel;
        public Button nextButton;

        public void OnClickNext()
        {
            nextButton.interactable = false;
            gameCmdFactory.NextActionCmd(gameManagerViewModel).Execute();
        }
    }
}