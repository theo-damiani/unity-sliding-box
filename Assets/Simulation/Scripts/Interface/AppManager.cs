using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
using TMPro;

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
    [SerializeField] private Vector3Variable boxVelocity;
    [SerializeField] private DraggableVector boxVelocityVector;

    [Header("Force Pushing Object 1")]
    [SerializeField] private BoolVariable pushIsActive;
    [SerializeField] private BoolVariable pushIsInteractive;
    [SerializeField] private Vector3Variable pushForce;
    [SerializeField] private BoolVariable pushShowVector;
    [SerializeField] private GameObject pushShowLabel;
    [SerializeField] private BoolVariable pushShowEquation;
    [SerializeField] private DraggableVector pushVector;

    [Header("Friction 1")]
    [SerializeField] private FloatVariable maxFrictionCoeff;
    [SerializeField] private Vector frictionVector;
    [SerializeField] private Slider frictionSlider;

    [Header("Timer")]
    [SerializeField] private RectTransform timerToggle;

    [Header("Infinite distance measure")]
    [SerializeField] private InfiniteTimeLine infiniteTimeLine;



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
        playButton.gameObject.SetActive(currentAffordances.showPlayPauseButton);
        resetButton.gameObject.SetActive(currentAffordances.showResetButton);
        
        playButton.GetComponent<PlayButton>().PlayWithoutRaising();

        // ============= Box =============
        // need to set mainObject
        mainObject.localPosition = currentAffordances.physicalObject.initialPosition.ToVector3();

        boxVelocity.Value = currentAffordances.physicalObject.initialVelocity.ToVector3();
        boxVelocityVector.gameObject.SetActive(currentAffordances.physicalObject.showVelocityVector);
        boxVelocityVector.SetInteractable(currentAffordances.physicalObject.velocityVectorIsInteractive);
        boxVelocityVector.Redraw();

        mainObject.GetComponent<Rigidbody>().isKinematic = false;
        mainObject.GetComponent<Rigidbody>().velocity = boxVelocity.Value;

        // ============= Push =============
        pushIsActive.Value = currentAffordances.pushForce.isActive;
        pushShowVector.Value = currentAffordances.pushForce.showVector;

        pushForce.Value = Vector3.right * currentAffordances.pushForce.initialMagnitude;
        //pushForce.Value = Quaternion.Euler(currentAffordances.physicalObject.initialRotation.ToVector3()) * pushForce.Value;

        pushShowEquation.Value = currentAffordances.pushForce.showEquation;
        pushShowLabel.SetActive(currentAffordances.pushForce.showLabel);
        pushIsInteractive.Value = currentAffordances.pushForce.isInteractive;
        pushVector.SetInteractable(currentAffordances.pushForce.isConfigurable);

        // ============= Friction =============

        // TODO:
        maxFrictionCoeff.Value = 0.2f;
        frictionSlider.SetValueWithoutNotify(0.2f);
        frictionVector.components.Value = Vector3.zero;
        frictionVector.Redraw();
        
        // ============= Camera =============
        Vector3 cameraPos = currentAffordances.camera.position.ToVector3();
        cameraLockingToggle.SetWithoutRaising(currentAffordances.camera.isLockedOnObject);

        Slider zoomSlider = cameraZoomSlider.GetComponent<Slider>();
        float minDistanceToObject = (mainObject.localScale.x + mainObject.localScale.y + mainObject.localScale.z)/3;
        // Init camera
        mainCamera.InitCamera(
            mainObject,
            cameraPos,
            currentAffordances.camera.isLockedOnObject,
            minDistanceToObject,
            zoomSlider
        );

        cameraControls.gameObject.SetActive(currentAffordances.camera.showCameraControl);

        // ============= Path Renderer =============
        showPath.Value = currentAffordances.physicalObject.showTrace;
        showPathToggle.gameObject.SetActive(currentAffordances.physicalObject.showTraceIsInteractive);
        showPathToggle.GetComponent<ToggleStartActivation>().SetToggleVisibility(currentAffordances.physicalObject.showTrace);

        // ============= Timer Toggle =============
        timerToggle.gameObject.SetActive(true);
        timerToggle.GetComponent<ToggleIcons>().SetToFalse();
        infiniteTimeLine.InitTimeLine();
    }
}
