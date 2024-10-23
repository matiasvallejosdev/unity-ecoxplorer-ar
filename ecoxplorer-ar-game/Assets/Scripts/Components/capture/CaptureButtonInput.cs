using UnityEngine;
using ViewModel;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using R3;

public class CaptureButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameCmdFactory gameCmdFactory;
    public GameManagerViewModel gameManager;
    public ApiServiceViewModel apiService;
    public StorageServiceViewModel storageService;
    public StorytellerViewModel storytellerViewModel;
    public RecognitionServiceViewModel recognitionService;
    public VoiceGeneratorViewModel voiceGeneratorViewModel;

    public Button button;
    public AudioSource audioSource;
    public float requiredPressDuration = 4.0f; // Adjust this value as needed

    private bool isButtonPressed = false;

    void Start()
    {
        // public ARTrackedImageManager arTrackedImageManager;

        // recognitionManager.recognitionActive.Subscribe(active => {
        //     button.interactable = active;
        // }).AddTo(this);
        // recognitionManager.recognitionActive.Value = true;
        gameManager.isLoadingPlay.Subscribe(isLoadingPlay => OnLoadingPlay(isLoadingPlay));
    }

    private void OnPlayStory(bool isPlayingStory)
    {
    }

    private void OnLoadingPlay(bool isLoadingPlay)
    {
        button.interactable = !isLoadingPlay;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonUp();
    }

    public void OnButtonDown()
    {
        if (!button.enabled || !button.interactable || gameManager.isLoadingPlay.Value || gameManager.isPlayingStory.Value)
        {
            return;
        }
        if (!isButtonPressed)
        {
            isButtonPressed = true;
            gameManager.isScanning.Value = true;
            StartCoroutine(ButtonPressedCoroutine());
        }
    }

    public void OnButtonUp()
    {
        gameManager.isScanning.Value = false;
        isButtonPressed = false;
        StopAllCoroutines();
        audioSource.Stop();
    }

    private IEnumerator ButtonPressedCoroutine()
    {
        float pressedTime = 0f;
        audioSource.Play();

        while (isButtonPressed && pressedTime < requiredPressDuration)
        {
            pressedTime += Time.deltaTime;
            yield return null;
        }

        if (isButtonPressed && pressedTime >= requiredPressDuration)
        {
            Play();
        }

        audioSource.Stop();
    }

    private void Play()
    {
        var arCamera = Camera.main;
        var playInput = new PlayCmdDto()
        {
            arCamera = arCamera,
            apiService = apiService,
            gameManagerViewModel = gameManager,
            storageService = storageService,
            recognitionService = recognitionService,
            storytellerViewModel = storytellerViewModel,
            voiceGeneratorViewModel = voiceGeneratorViewModel,
        };
        var recognizeTurnCmd = gameCmdFactory.PlayInput(playInput);
        OnButtonUp();
        gameManager.isLoadingPlay.Value = true;
        recognizeTurnCmd.Execute();
    }
}
