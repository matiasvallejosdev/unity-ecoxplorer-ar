using System;
using System.Collections;
using System.Collections.Generic;
using Commands;
using Contracts;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;

[CreateAssetMenu(fileName = "Menu Command Factory", menuName = "Factory/Menu Factory")]
public class MenuCmdFactory : ScriptableObject
    {
    public MainMenuStartCmd MainMenuStartCmd(MenuManageViewModel menuManageViewModel)
    {
        return new MainMenuStartCmd(menuManageViewModel);
    }

    public SessionStartActionCmd MainMenuSessionStartCmd(SessionStartDto sessionStartDto)
    {
        sessionStartDto.levelBuilderGateway = new LevelBuilder();
        return new SessionStartActionCmd(sessionStartDto);
    }
}
