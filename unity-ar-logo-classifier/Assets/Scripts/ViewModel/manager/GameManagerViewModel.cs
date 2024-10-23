using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Game Manager", menuName = "Manager/Game Manager")]
    public class GameManagerViewModel : ScriptableObject
    {
        [Header("Game")]
        public PlayerDataViewModel playerData;
        public List<PlayerStoriesViewModel> playerStories = new List<PlayerStoriesViewModel>();

        [Header("Settings")]
        public int textureWidth;
        public int textureHeight;
        public bool saveLocalScreenshoot = false;

        [Header("AR Settings")]
        public ARObjectViewModel[] recogntionObjects;
        public int maxNumberOfImages;
        public XRReferenceImageLibrary runtimeImageLibrary;

        [Header("Runtime")]
        public ReactiveProperty<int> storiesCompleted = new(0);
        public ReactiveProperty<string> languageSelected = new("es");
        public ReactiveProperty<bool> isScanning = new(false);
        public ReactiveProperty<bool> isPlayingStory = new(false);
        public ReactiveProperty<bool> isLoadingPlay = new(false);
        public Subject<PlayerStoriesViewModel> OnPlayStory = new();
        public Subject<bool> OnFinishStory = new();

        //public ReactiveProperty<bool> recognitionActive = new(false);
        // public ReactiveProperty<bool> recognitionActive = new(false);
        //public Subject<bool> OnRecognitionStart { get; } = new Subject<bool>();
        //public ReactiveProperty<bool> arRecognitionEnable = new();
        // public ReactiveProperty<int> recognitionObjectsCount = new(0);
        // public ReactiveProperty<Texture> lastScreenshoot = new ReactiveProperty<Texture>();
        // public ReactiveProperty<EnvironmentalImageAnalysisResponse> environmentalImageAnalysisResponse   = new ReactiveProperty<EnvironmentalImageAnalysisResponse>();
    }
}
