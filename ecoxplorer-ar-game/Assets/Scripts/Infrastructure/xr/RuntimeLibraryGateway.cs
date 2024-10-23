using System;
using System.Collections;
using System.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure
{
    public class RuntimeLibraryGateway : IRuntimeLibraryGateway
    {
        public async Task<Result<Unit>> AddImageRuntimeLibrary(Texture2D screenShotTex, ARTrackedImageManager trackedImageManager, string imageRecognition)
        {
            if (screenShotTex == null)
            {
                Debug.LogError("Texture2D is null");
                return Result<Unit>.Failure(new ArgumentNullException(nameof(screenShotTex)));
            }

            try
            {
                await AddImageCoroutine(trackedImageManager, screenShotTex, imageRecognition);
                return Result<Unit>.Success(Unit.Default);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error adding image to runtime library: {ex.Message}");
                return Result<Unit>.Failure(ex);
            }
        }

        private Task AddImageCoroutine(ARTrackedImageManager trackedImageManager, Texture2D texture2D, string imageRecognition)
        {
            return Task.Run(() =>
            {
                if (!(trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary))
                {
                    throw new InvalidOperationException("TrackedImageManager's reference library is not mutable.");
                }

                Debug.Log("Adding image to the runtime library");

                // Define image
                string id = Guid.NewGuid().ToString();
                string imageName = $"{imageRecognition}_{id}";

                // Schedule the add image job
                var addImageJob = mutableLibrary.ScheduleAddImageWithValidationJob(texture2D, imageName, 0.1f); // Assume 0.1m physical size, adjust as necessary

                // Wait for the job to complete
                while (!addImageJob.jobHandle.IsCompleted)
                {
                    // Yield control back to Unity to prevent freezing
                    // This can be replaced with a proper async method if needed
                }

                addImageJob.jobHandle.Complete();

                if (addImageJob.status == AddReferenceImageJobStatus.Success)
                {
                    Debug.Log($"Successfully added or updated the image '{imageRecognition}' in the runtime reference image library.");
                }
                else
                {
                    string errorMessage = $"Failed to add the image '{imageRecognition}' to the runtime reference image library. Status: {addImageJob.status}";
                    throw new Exception(errorMessage);
                }
            });
        }
    }
}
