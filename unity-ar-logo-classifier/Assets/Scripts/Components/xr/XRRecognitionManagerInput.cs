using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ViewModel;
using Commands;
using UnityEngine.XR.ARFoundation;

namespace Components
{
    public class XRRecognitionManagerInput : MonoBehaviour
    {
        public XrCmdFactory xrCmdFactory;
        public GameManagerViewModel recognitionManager;
        public ARTrackedImageManager trackedImageManager;

        void Awake()
        {
            // xrCmdFactory.XRImageStart(recognitionManager, trackedImageManager).Execute();
        }
    }
}