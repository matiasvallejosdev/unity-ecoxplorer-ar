using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ViewModel;
using R3;
using Contracts;

public class XRStoryPointerPlaceDisplay : MonoBehaviour
{
    public GameManagerViewModel GameManagerViewModel;
    public GameObject item;

    public ARRaycastManager raycastManager;
    private Camera arCamera;
    public float placementDistance = 1.5f;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool positionSaved = false;

    void Start()
    {
        arCamera = Camera.main;
        GameManagerViewModel.OnClickScreenCenter.Subscribe(SavePosition);
        GameManagerViewModel.OnMatch.Subscribe(OnMatch).AddTo(this);
    }

    private void OnMatch(MatchReactiveDto matchReactiveDto)
    {
        if (!matchReactiveDto.isMatch || matchReactiveDto.isCancelledOperation)
            return;


        if (positionSaved)
        {
            Instantiate(item, savedPosition, savedRotation);
        }
    }

    public void SavePosition(Vector2 screenCenter)
    {
        // Create a list to store raycast hits
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Raycast from the center of the screen
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            // Get the hit pose
            Pose hitPose = hits[0].pose;
            savedPosition = hitPose.position;
            savedRotation = hitPose.rotation;
            positionSaved = true;
        }
        else
        {
            // If no plane is detected, save position at a fixed distance from the camera
            Vector3 cameraForward = arCamera.transform.forward;
            savedPosition = arCamera.transform.position + cameraForward * placementDistance;
            savedRotation = Quaternion.identity;
            positionSaved = true;
        }
    }
}
