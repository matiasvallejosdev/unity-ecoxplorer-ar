using System;
using System.Collections.Generic;
using Contracts;
using R3;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Game Manager", menuName = "Manager/Game Manager")]
    public class GameManagerViewModel : ScriptableObject
    {
        [Header("Instructions")]
        public string pressToPlayInstruction = "Manten presionado y captura";
        public string startInstruction = "Explora el entorno. Busca a tu alrededor, mantén presionado y captura.";
        public string endInstruction = "¡Felicidades! Has completado el juego.";
        public string wrongMatchInstruction = "Explore y vuelva a intentarlo en ";
        public string waitAfterStoryInstruction = "Bien hecho! Espere ";

        [Header("Settings")]
        public int timeLineGuideline = 10;
        public int storiesToComplete = 3;
        public int requiredPressDuration = 12;
        public int storyDelay = 12000;
        public int timeBetweenMatches = 10000;
        public int timeBetweenStories = 10000;
        public int textureWidth;
        public int textureHeight;
        public bool saveLocalScreenshoot = false;

        [Header("Session")]
        public List<TopicDataViewModel> topics = new List<TopicDataViewModel>();
        public ReactiveProperty<string> languageSelected = new("es");
        public TopicDataViewModel currentTopicData;
        public LevelDataViewModel currentLevelData;
        public CurrentStoryState currentStoryState = CurrentStoryState.Start;

        [Header("Game")]
        public GameScene currentScene = GameScene.MainMenu;
        public ReactiveProperty<string> actionIndications = new("");
        public Subject<MatchReactiveDto> OnMatch = new();
        public ReactiveProperty<bool> isSessionStarted = new(false);
        public ReactiveProperty<bool> isLoadingPlay = new(false);
        public ReactiveProperty<bool> isGameEnded = new(false);

        [Header("Player")]
        public PlayerDataViewModel playerData;
        public List<PlayerStoriesViewModel> playerStories = new List<PlayerStoriesViewModel>();
        public ReactiveProperty<bool> isPlayingStory = new(false);
        public ReactiveProperty<int> storiesCompleted = new(0);
        public PlayerStoriesViewModel currentPlayerStory;
        public Subject<PlayerStoriesViewModel> OnStoryStart = new();
        public Subject<bool> OnStoryLoad = new();
        public Subject<bool> OnStoryEnd = new();

        [Header("AR Settings")]
        public ARObjectViewModel[] recogntionObjects;
        public int maxNumberOfImages;
        public XRReferenceImageLibrary runtimeImageLibrary;

        [Header("Runtime")]
        public Subject<bool> OnGameStart = new();
        public Subject<bool> OnGameEnd = new();
        public Subject<Vector2> OnClickScreenCenter = new();
        public Subject<MessageDto> OnMessage = new();
        public Subject<bool> OnCapture = new();
    }
}
