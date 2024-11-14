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

[CreateAssetMenu(fileName = "Game Command Factory", menuName = "Factory/Game Factory")]
public class GameCmdFactory : ScriptableObject
{
    public StartActionCmd StartActionCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new StartActionCmd(gameManagerViewModel);
    }

    public EndActionCmd EndActionCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new EndActionCmd(gameManagerViewModel);
    }

    public MatchActionCmd MatchActionCmd(MatchCmdDto matchCmdDto)
    {
        matchCmdDto.StorageImageGateway = new StorageImageGateway();
        matchCmdDto.MatchGateway = new MatchGateway();
        return new MatchActionCmd(matchCmdDto);
    }

    public NextActionCmd NextActionCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new NextActionCmd(gameManagerViewModel);
    }

    public TransitionNextActionCmd TransitionNextActionCmd(GameManagerViewModel gameManagerViewModel)
    {
        return new TransitionNextActionCmd(gameManagerViewModel);
    }
}
