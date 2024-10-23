using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;

[CreateAssetMenu(fileName = "Game Command Factory", menuName = "Factory/Game Factory")]
public class GameCmdFactory : ScriptableObject
{
    public PlayActionCmd PlayInput(PlayCmdDto playInputDto)
    {
        playInputDto.AudioGateway = new AudioGateway();
        playInputDto.ImageAnalysisGateway = new ImageRecognitionGateway();
        playInputDto.StorageImageGateway = new StorageImageGateway();
        playInputDto.RuntimeLibraryGateway = new RuntimeLibraryGateway();
        playInputDto.StorytellerGateway = new StorytellerGateway();
        playInputDto.VoiceGeneratorGateway = new VoiceGeneratorGateway();
        return new PlayActionCmd(playInputDto);
    }

    public StoryActionCmd StoryCmd(GameManagerViewModel gameManagerViewModel, PlayerStoriesViewModel story)
    {
        return new StoryActionCmd(gameManagerViewModel, story);
    }

    public StoryFinishActionCmd FinishPlayStoryCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new StoryFinishActionCmd(gameManagerViewModel);
    }
}

public class PlayCmdDto
{
    public Camera arCamera { get; set; }
    public GameManagerViewModel gameManagerViewModel { get; set; }
    public ApiServiceViewModel apiService { get; set; }
    public StorageServiceViewModel storageService { get; set; }
    public RecognitionServiceViewModel recognitionService { get; set; }
    public StorytellerViewModel storytellerViewModel { get; set; }
    public VoiceGeneratorViewModel voiceGeneratorViewModel { get; set; }

    // public ARTrackedImageManager trackedImageManager { get; set; }
    public IImageRecognitionGateway ImageAnalysisGateway { get; set; }
    public IStorageImageGateway StorageImageGateway { get; set; }
    public IRuntimeLibraryGateway RuntimeLibraryGateway { get; set; }
    public IStorytellerGateway StorytellerGateway { get; set; }
    public IVoiceGeneratorGateway VoiceGeneratorGateway { get; set; }
    public IAudioGateway AudioGateway { get; set; }
}