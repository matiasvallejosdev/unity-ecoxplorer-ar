using System.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure
{
    public interface IRuntimeLibraryGateway
    {
        public Task<Result<Unit>> AddImageRuntimeLibrary(Texture2D screenShotTex, ARTrackedImageManager trackedImageManager, string imageRecognition);
    }
}