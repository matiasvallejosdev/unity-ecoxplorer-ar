using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;
using System.Linq;
using System;

namespace Components
{
    public class XRImageObjectDisplay : MonoBehaviour
    {
        public ARTrackedImageManager trackedImageManager;
        public GameManagerViewModel recognitionManager;

        private Dictionary<string, GameObject> _instantiatedARPrefabs = new Dictionary<string, GameObject>();

        void OnEnable() => trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        void OnDisable() => trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            // if (recognitionManager.recognitionObjectsCount.Value <= 0)
            //     return;

            try
            {
                // Handle added images
                foreach (ARTrackedImage trackedImage in eventArgs.added)
                {
                    InstanceARPrefab(trackedImage);
                }

                // Handle updated images
                foreach (ARTrackedImage trackedImage in eventArgs.updated)
                {
                    // Use the image's name or GUID as the key
                    string key = trackedImage.referenceImage.guid.ToString();

                    if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && _instantiatedARPrefabs.ContainsKey(key))
                    {
                        UpdateARPrefab(_instantiatedARPrefabs[key], trackedImage);
                    }
                    else if (_instantiatedARPrefabs.ContainsKey(key))
                    {
                        _instantiatedARPrefabs[key].SetActive(false);
                    }
                }

                // Handle removed images
                foreach (ARTrackedImage trackedImage in eventArgs.removed)
                {
                    // Use the image's name or GUID as the key
                    string key = trackedImage.referenceImage.guid.ToString();

                    if (_instantiatedARPrefabs.ContainsKey(key))
                    {
                        // Optionally, you could Destroy the GameObject instead of just deactivating it
                        GameObject toRemove = _instantiatedARPrefabs[key];
                        _instantiatedARPrefabs.Remove(key);
                        Destroy(toRemove);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Horrible things happened! (" + e.ToString() + ")");
            }
        }

        private void InstanceARPrefab(ARTrackedImage trackedImage)
        {
            // Use the image name or GUID as the key
            string key = trackedImage.referenceImage.guid.ToString();

            string name = "";
            // string name = recognitionManager.environmentalImageAnalysisResponse.Value.recognition.ToString().ToLower();

            var arObject = recognitionManager.recogntionObjects.FirstOrDefault(obj => obj.objectLabel.ToString().ToLower() == name);
            if (arObject == null)
            {
                Debug.LogError("No object found with name: " + name);
                return;
            }

            if (!_instantiatedARPrefabs.ContainsKey(key))
            {
                Debug.Log($"Tracked image with name {trackedImage.referenceImage.name} and id {trackedImage.referenceImage.guid}");
                Debug.Log($"Instance AR Prefab with name {arObject.objectLabel}");

                var prefab = Instantiate(arObject.objectPrefab);
                _instantiatedARPrefabs.Add(key, prefab);

                UpdateARPrefab(prefab, trackedImage);
            }
        }

        private void UpdateARPrefab(GameObject prefab, ARTrackedImage trackedImage)
        {
            prefab.transform.position = trackedImage.transform.position;
            prefab.SetActive(true);
        }
    }
}
