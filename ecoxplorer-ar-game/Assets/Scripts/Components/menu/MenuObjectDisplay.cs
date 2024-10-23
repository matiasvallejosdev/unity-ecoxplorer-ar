using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class MenuObjectDisplay : MonoBehaviour
    {
        public GameObject playPanel;
        public GameObject subtitlePanel;
        public GameObject screenCapturePanel;
        public GameObject greetingPanel;
        public GameObject topPanel;

        void Start()
        {
            topPanel.SetActive(false);
            subtitlePanel.SetActive(false);
            screenCapturePanel.SetActive(false);
            greetingPanel.SetActive(true);
            playPanel.SetActive(false);
        }

        public void OnStartGame()
        {
            greetingPanel.SetActive(false);
            playPanel.SetActive(true);
            subtitlePanel.SetActive(true);
            screenCapturePanel.SetActive(true);
            topPanel.SetActive(true);
        }
    }
}
