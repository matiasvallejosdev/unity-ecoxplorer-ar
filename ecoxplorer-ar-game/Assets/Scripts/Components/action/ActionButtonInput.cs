using UnityEngine;
using ViewModel;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using R3;
using UnityEngine.InputSystem;
using Contracts;

public class ActionButtonInput : MonoBehaviour
{
    public GameCmdFactory gameCmdFactory;
    public GameManagerViewModel gameManager;
    public StorageServiceViewModel storageService;
    public MatchServiceViewModel matchService;

    private bool isButtonPressed;
    private Vector2 screenCenter;
    private InputAction touchPressAction;
    private InputAction mousePressAction;
    private InputActionMap inputActions;
    private float timeToWaitAfterStoryEnd;
    private float timeBetweenMatchWrong;
    private float requiredPressDuration;
    private bool isBlockedByMatch;
    private bool isBlockedByStory;
    private string wrongMatchInstruction;
    private string waitAfterStoryInstruction;

    private void Start()
    {
        timeToWaitAfterStoryEnd = gameManager.timeBetweenStories;
        timeBetweenMatchWrong = gameManager.timeBetweenMatches;
        requiredPressDuration = gameManager.requiredPressDuration;
        wrongMatchInstruction = gameManager.wrongMatchInstruction;
        waitAfterStoryInstruction = gameManager.waitAfterStoryInstruction;
        InitializeInputActions();
        SubscribeToEvents();
    }

    private void InitializeInputActions()
    {
        inputActions = new InputActionMap();
        touchPressAction = inputActions.AddAction("touchPress");
        mousePressAction = inputActions.AddAction("mousePress");

        touchPressAction.Enable();
        mousePressAction.Enable();
    }

    private void SubscribeToEvents()
    {
        gameManager.OnMatch.Subscribe(OnMatch).AddTo(this);
        gameManager.OnStoryEnd.Subscribe(OnStoryEnd).AddTo(this);
        gameManager.isGameEnded.Subscribe(OnGameEnd).AddTo(this);
    }

    private void OnGameEnd(bool isGameEnded)
    {
        if (isGameEnded)
        {
            StopAllCoroutines();
            isBlockedByMatch = false;
            isBlockedByStory = false;
        }
    }

    private void OnStoryEnd(bool isStoryEnd)
    {
        StartCoroutine(WaitAfterStoryEnd());
    }

    private void OnMatch(MatchReactiveDto matchReactiveDto)
    {
        OnButtonUp();
        if (!matchReactiveDto.isMatch)
        {
            StartCoroutine(BlockAfterMatch());
        }
    }

    private IEnumerator BlockAfterMatch()
    {
        isBlockedByMatch = true;
        float timeLeft = timeBetweenMatchWrong;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            gameManager.actionIndications.Value = $"{wrongMatchInstruction} {timeLeft:F1}s";
            yield return null;
        }

        isBlockedByMatch = false;
        gameManager.actionIndications.Value = gameManager.pressToPlayInstruction;
    }

    private IEnumerator WaitAfterStoryEnd()
    {
        isBlockedByStory = true;
        float timeLeft = timeToWaitAfterStoryEnd;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            gameManager.actionIndications.Value = $"{waitAfterStoryInstruction} {timeLeft:F1}s";
            yield return null;
        }

        isBlockedByStory = false;
        gameManager.actionIndications.Value = gameManager.pressToPlayInstruction;
    }

    private void Update()
    {
        if (!gameManager.isGameEnded.Value)
        {
            HandleTouchInput();
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;

        var touch = touchscreen.primaryTouch;
        if (touch.press.wasPressedThisFrame && CanBePressed() && !gameManager.isGameEnded.Value)
        {
            Debug.Log("Touch Began");
            OnButtonDown();
        }
        else if (!touch.isInProgress && isButtonPressed)
        {
            Debug.Log("Touch Ended");
            OnButtonUp();
        }
    }

    private bool CanBePressed()
    {
        Debug.Log("CanBePressed: " + gameManager.isPlayingStory.Value + " " + gameManager.isLoadingPlay.Value);
        // If the story is playing, the game is loading, or we're in a blocking period
        if (gameManager.isPlayingStory.Value ||
            gameManager.isLoadingPlay.Value ||
            isBlockedByMatch ||
            isBlockedByStory)
        {
            Debug.Log("Cannot press button - Story is playing, loading, or in cooldown");
            return false;
        }
        return true;
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            gameManager.actionIndications.Value = "Manten presionado";
            OnButtonDown();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Debug.Log("Mouse Up");
            OnButtonUp();
        }
    }

    public void OnButtonDown()
    {
        if (isButtonPressed) return;

        isButtonPressed = true;
        gameManager.isSessionStarted.Value = true;
        StartCoroutine(ButtonPressedCoroutine());
    }

    public void OnButtonUp()
    {
        gameManager.isSessionStarted.Value = false;
        isButtonPressed = false;
        StopAllCoroutines();
        if (gameManager.actionIndications.Value == "Manten presionado")
        {
            var pressToPlay = gameManager.pressToPlayInstruction;
            gameManager.actionIndications.Value = pressToPlay;
        }
    }
    private IEnumerator ButtonPressedCoroutine()
    {
        ImageMatchInput();

        float pressedTime = 0f;
        while (isButtonPressed && pressedTime < requiredPressDuration)
        {
            pressedTime += Time.deltaTime;
            if (!gameManager.isSessionStarted.Value)
            {
                break;
            }
            yield return null;
        }
    }

    private void ImageMatchInput()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        var matchInput = new MatchCmdDto
        {
            arCamera = Camera.main,
            gameManagerViewModel = gameManager,
            storageService = storageService,
            matchService = matchService
        };

        var recognizeTurnCmd = gameCmdFactory.MatchActionCmd(matchInput);
        gameManager.isLoadingPlay.Value = true;
        gameManager.OnClickScreenCenter.OnNext(screenCenter);
        recognizeTurnCmd.Execute();
    }

    private void OnDisable()
    {
        touchPressAction?.Disable();
        mousePressAction?.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }
}
