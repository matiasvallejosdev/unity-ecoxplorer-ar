using UnityEngine.UI;
using ViewModel;
using R3;
using UnityEngine;
using Newtonsoft.Json;
using Contracts;

public class ActionMatchDisplay : MonoBehaviour
{
    public GameManagerViewModel gameManagerViewModel;

    public void Start()
    {
        gameManagerViewModel.OnMatch.Subscribe(OnMatch).AddTo(this);
    }

    private void OnMatch(MatchReactiveDto matchReactiveDto)
    {
        var msg = matchReactiveDto.isMatch ? "Match Success. Story will start soon." : "No Match, it was cancelled or failed.";
        Debug.Log(msg);

        if (!matchReactiveDto.isMatch)
        {
            var guidenceCount = gameManagerViewModel.currentLevelData.levelGuidance.Count;
            // Use current time as seed for random number generator
            var rand = new System.Random(System.DateTime.Now.Millisecond);
            var randomIndex = rand.Next(0, guidenceCount);
            Debug.Log($"Random index: {randomIndex}");
            var guidanceStep = gameManagerViewModel.currentLevelData.levelGuidance[randomIndex];
            gameManagerViewModel.OnMessage.OnNext(new MessageDto(guidanceStep, Color.yellow));
        }
    }
}
