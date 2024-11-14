using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;
using Commands;

[CreateAssetMenu(fileName = "Story Command Factory", menuName = "Factory/Story Factory")]
public class StoryCmdFactory : ScriptableObject
{
    public StoryLoadActionCmd StoryLoadActionCmd(StoryLoadCmdDto storyLoadCmdDto)
    {
        storyLoadCmdDto.AudioGateway = new AudioGateway();
        storyLoadCmdDto.ImageAnalysisGateway = new ImageRecognitionGateway();
        storyLoadCmdDto.StorytellerGateway = new StorytellerGateway();
        storyLoadCmdDto.VoiceGeneratorGateway = new VoiceGeneratorGateway();
        return new StoryLoadActionCmd(storyLoadCmdDto);
    }

    public StoryStartActionCmd StoryPlayCmd(GameManagerViewModel gameManagerViewModel, PlayerStoriesViewModel story)
    {
        return new StoryStartActionCmd(gameManagerViewModel, story);
    }

    public StoryFinishActionCmd StoryFinishCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new StoryFinishActionCmd(gameManagerViewModel);
    }
}
