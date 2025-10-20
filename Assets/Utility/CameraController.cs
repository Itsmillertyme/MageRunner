using Cinemachine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    [SerializeField] List<CinemachineVirtualCamera> vCams;
    [SerializeField] TextMeshProUGUI primHUDCamTMP;
    int activeCam;
    int CAM_PRIORITY_OFFSET;

    private void Awake() {
        //Initialize Cameras              
        int priority = 0;
        CAM_PRIORITY_OFFSET = vCams.Count;
        for (int i = 0; i < vCams.Count; i++) {
            if (vCams[i].Priority > priority) {
                activeCam = i;
                priority = vCams[i].Priority;
            }
        }

        //Initialize TMP
        if (primHUDCamTMP != null) primHUDCamTMP.text = "Camera Position: " + activeCam;

    }

    private void Update() {

        //Debug.Log("Active Camera Position: " + activeCam);
    }

    public void CycleCameraPosition(InputAction.CallbackContext context) {

        //reset current cam priority
        vCams[activeCam].Priority -= CAM_PRIORITY_OFFSET;

        //cycle active cam
        activeCam = ++activeCam % vCams.Count;

        //set HUD
        primHUDCamTMP.text = "Camera Position: " + activeCam;

        //set current cam priority
        vCams[activeCam].Priority += CAM_PRIORITY_OFFSET;
    }

    public void SetToCutSceneCamera(Transform target) {
        vCams[activeCam].Priority -= CAM_PRIORITY_OFFSET;

        //set cutscene cam priority
        vCams[1].Priority += CAM_PRIORITY_OFFSET;
        vCams[1].GetComponent<CinemachineVirtualCamera>().Follow = target;
        vCams[1].GetComponent<CinemachineVirtualCamera>().LookAt = target;
    }

    public void SetToCurrentCamera(Transform target) {
        int currentActiveIndex = -1;
        int currentHighestPriority = Int32.MinValue;
        for (int i = 0; i < vCams.Count; i++) {
            if (vCams[i].Priority > currentHighestPriority) {
                currentActiveIndex = i;
                currentHighestPriority = vCams[i].Priority;
            }
        }

        vCams[currentActiveIndex].Priority -= CAM_PRIORITY_OFFSET;
        vCams[currentActiveIndex].GetComponent<CinemachineVirtualCamera>().Follow = target;
        vCams[currentActiveIndex].GetComponent<CinemachineVirtualCamera>().LookAt = target;

        vCams[activeCam].Priority += CAM_PRIORITY_OFFSET;
        vCams[activeCam].GetComponent<CinemachineVirtualCamera>().Follow = target;
        vCams[activeCam].GetComponent<CinemachineVirtualCamera>().LookAt = target;
    }
}
