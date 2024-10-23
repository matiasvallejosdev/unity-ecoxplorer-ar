
using UnityEngine.UI;
using ViewModel;
using R3;
using UnityEngine;

public class CaptureButtonDisplay : MonoBehaviour
{
    public Button button;
    public GameManagerViewModel gameManagerViewModel;

    public void Start()
    {
        gameManagerViewModel.isPlayingStory.Subscribe(isPlayingStory => OnPlayStory(isPlayingStory));
        gameManagerViewModel.isLoadingPlay.Subscribe(isLoadingPlay => OnLoadingPlay(isLoadingPlay));
    }

    private void OnPlayStory(bool isPlayingStory)
    {
        button.interactable = !isPlayingStory;
    }

    private void OnLoadingPlay(bool isLoadingPlay)
    {
        button.interactable = !isLoadingPlay;
    }
}
