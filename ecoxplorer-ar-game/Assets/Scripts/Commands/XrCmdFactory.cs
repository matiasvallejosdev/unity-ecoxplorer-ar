using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Commands
{
    [CreateAssetMenu(fileName = "XR Command Factory", menuName = "Factory/XR Factory")]
    public class XrCmdFactory : ScriptableObject
    {
        public XRImageStartCmd XRImageStart(GameManagerViewModel recognitionManager, ARTrackedImageManager trackedImageManager)
        {
            return new XRImageStartCmd(recognitionManager, trackedImageManager);
        }
    }
}