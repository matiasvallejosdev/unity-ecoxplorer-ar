using ViewModel;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMotionDisplay : MonoBehaviour
{
    public GameManagerViewModel gameManagerViewModel;
    public AudioSource audioSourceCapture;
    public AudioSource audioSourceTap;
    public AudioClip captureAudio;
    public AudioClip tapAudio;

    public void Start()
    {
        gameManagerViewModel.OnCapture.Subscribe(OnCapture).AddTo(this);
    }

    private void OnCapture(bool isCapture)
    {
        Debug.Log("OnCapture"); 
        audioSourceCapture.clip = captureAudio;
        audioSourceCapture.Play();
    }

    private void OnTap()
    {
        audioSourceTap.clip = tapAudio;
        audioSourceTap.Play();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame || 
            Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            OnTap();
        }
    }
}
