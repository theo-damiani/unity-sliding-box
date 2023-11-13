using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;

public class AppManager : Singleton<AppManager>
{
    [Header("Affordances")]
    [SerializeField] private Affordances defaultAffordances;
    private Affordances currentAffordances;

    [Header("Camera")]
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private RectTransform cameraControls;
    [SerializeField] private ToggleIcons cameraLockingToggle;
    [SerializeField] private RectTransform cameraZoomSlider;

    [Header("Main App Controls")]
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform resetButton;

    [Header("Main object variables")]
    [SerializeField] private Transform mainObject;
    [SerializeField] private RectTransform showPathToggle;
    [SerializeField] private BoolVariable showPath;
    public override void Awake()
    {
        base.Awake();
        
        currentAffordances = Instantiate(defaultAffordances);
        ResetApp();

        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
            WebGLInput.captureAllKeyboardInput = false;
        #endif
    }

    public void ResetAppFromJSON(string affordanceJson)
    {
        currentAffordances = Instantiate(defaultAffordances);
        JsonUtility.FromJsonOverwrite(affordanceJson, currentAffordances);
        ResetApp();
    }

    public void ResetApp()
    {
        // ============= Main control =============
        playButton.gameObject.SetActive(currentAffordances.showPlayButton);
        resetButton.gameObject.SetActive(currentAffordances.showResetButton);
        
        playButton.GetComponent<PlayButton>().PlayWithoutRaising();
        
        // ============= Camera =============
        Vector3 cameraPos = currentAffordances.camera.position.ToVector3();
        cameraLockingToggle.SetWithoutRaising(currentAffordances.camera.isLockedOnObject);

        Slider zoomSlider = cameraZoomSlider.GetComponent<Slider>();
        CameraManager cameraManager = mainCamera.GetComponent<CameraManager>();
        float minDistanceToObject = (mainObject.localScale.x + mainObject.localScale.y + mainObject.localScale.z)/3;
        // Init camera
        mainCamera.InitCamera(
            cameraPos,
            currentAffordances.camera.isLockedOnObject,
            minDistanceToObject,
            zoomSlider
        );
        // Init zoom slider
        float distanceToObject = (cameraPos - cameraManager.target.localPosition).magnitude;
        // float zoomScale = Mathf.Clamp(distanceToObject, minDistanceToObject, cameraManager.GetSliderMax());
        zoomSlider.minValue = 1;
        zoomSlider.maxValue = cameraManager.GetSliderMax();
        // zoomSlider.SetValueWithoutNotify(cameraManager.CameraToSlider(zoomScale));

        cameraControls.gameObject.SetActive(currentAffordances.camera.showCameraControl);

        // ============= Path Renderer =============
        showPath.Value = currentAffordances.physicalObject.showTrace;
        showPathToggle.gameObject.SetActive(currentAffordances.physicalObject.showTraceIsInteractive);
        showPathToggle.GetComponent<ToggleStartActivation>().SetToggleVisibility(currentAffordances.physicalObject.showTrace);
    }
}
