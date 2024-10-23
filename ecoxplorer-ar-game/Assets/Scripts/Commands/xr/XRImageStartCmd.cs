using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Commands
{
    public class XRImageStartCmd : ICommand
    {
        public GameManagerViewModel recognitionManager;
        public ARTrackedImageManager trackedImageManager;

        public XRImageStartCmd(GameManagerViewModel recognitionManagerViewModel, ARTrackedImageManager trackedImageManager)
        {
            this.recognitionManager = recognitionManagerViewModel;
            this.trackedImageManager = trackedImageManager;
        }

        public void Execute()
        {
            Debug.Log("Creating Runtime Mutable Image Library");
            var lib = trackedImageManager.CreateRuntimeLibrary(recognitionManager.runtimeImageLibrary);
            trackedImageManager.referenceLibrary = lib;

            trackedImageManager.requestedMaxNumberOfMovingImages = recognitionManager.maxNumberOfImages;
            //recognitionManager.recognitionActive.Value = true;

            // recognitionManager.recognitionObjectsCount.Value = 0;
        }
    }
}