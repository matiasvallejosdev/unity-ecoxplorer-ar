using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;

public class MainMenuStartInput : MonoBehaviour
{
    public MenuManageViewModel menuManageViewModel;
    public MenuCmdFactory menuCmdFactory;

    public GameObject mainMenu;
    public GameObject topicsMenu;
    public GameObject footerMenu;

    void Start()
    {
        menuCmdFactory.MainMenuStartCmd(menuManageViewModel).Execute();

        mainMenu.SetActive(true);
        footerMenu.SetActive(true);
        topicsMenu.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        mainMenu.SetActive(false);
        footerMenu.SetActive(false);
        topicsMenu.SetActive(true);
    }
}
