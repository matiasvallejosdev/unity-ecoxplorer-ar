

using UnityEngine;
using ViewModel;

public class MainMenuStartCmd : ICommand
{
    private MenuManageViewModel _menuManageViewModel;

    public MainMenuStartCmd(MenuManageViewModel menuManageViewModel)
    {
        _menuManageViewModel = menuManageViewModel;
    }

    public void Execute()
    {
        Debug.Log("MainMenuStartCmd. Iniciando sesion");
        _menuManageViewModel.isLoading.Value = false;
    }
}