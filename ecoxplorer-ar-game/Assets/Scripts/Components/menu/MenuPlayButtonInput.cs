using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MenuPlayButtonInput : MonoBehaviour
    {
        public GameCmdFactory gameCmdFactory;
        public GameManagerViewModel gameManagerViewModel;

        void OnClick()
        {
            gameCmdFactory.StartActionCmd(gameManagerViewModel).Execute();
        }
    }
}
