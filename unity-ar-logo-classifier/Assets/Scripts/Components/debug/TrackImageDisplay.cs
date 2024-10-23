using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

namespace Components
{
    public class TrackImageDisplay : MonoBehaviour
    {
        public GameManagerViewModel recognitionManager;
        public ARTrackedImageManager aRTrackedImageManager;
        public TextMeshProUGUI imageTrackedLabel;

        void Start(){
            aRTrackedImageManager.trackedImagesChanged += OnImageChange;
            imageTrackedLabel.text = "Brand/Logo";
        }
        void OnDisable() => aRTrackedImageManager.trackedImagesChanged -= OnImageChange;

        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            // if (recognitionManager.recognitionObjectsCount.Value <= 0)
            //     return;

            foreach (var trackImage in args.added)
            {
                imageTrackedLabel.text = $"{PerformName(trackImage.referenceImage.name)}";
            }
            foreach (var trackImage in args.updated)
            {
                imageTrackedLabel.text = $"{PerformName(trackImage.referenceImage.name)}";
            }
            foreach (var trackImage in args.removed)
            {
                imageTrackedLabel.text = $"{PerformName("")}";
            }
        }

        private void LogTrackedImage(ARTrackedImage trackedImage)
        {
            imageTrackedLabel.text = $"{PerformName(trackedImage.referenceImage.name)}";
        }

        private string PerformName(string name)
        {
            if (name == null || name == "") return "Brand/Logo";

            int underscoreIndex = name.IndexOf('_'); // Find the index of the first underscore

            if (underscoreIndex != -1) // Check if the underscore was found
            {
                string extractedName = name.Substring(0, underscoreIndex); // Extract from start to underscor
                return extractedName; // Return the extracted name
            }
            return "Brand/Logo"; // Return a default value
        }
    }

}
