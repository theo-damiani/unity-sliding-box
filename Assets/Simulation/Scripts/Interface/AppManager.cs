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
    [SerializeField] private GameObject boxVelocityLabel;

    [Header("Force Pushing Object 1")]
    [SerializeField] private BoolVariable pushIsActive;
    [SerializeField] private BoolVariable pushIsInteractive;
    [SerializeField] private Vector3Variable pushForce;
    [SerializeField] private BoolVariable pushShowVector;
    [SerializeField] private GameObject pushShowLabel;
    [SerializeField] private BoolVariable pushShowEquation;
    [SerializeField] private DraggableVector pushVector;
    [SerializeField] private ToggleIcons pushForceToggle;

    [Header("Friction 1")]
    [SerializeField] private FloatVariable staticFrictionCoeff;
    [SerializeField] private FloatVariable kineticFrictionCoeff;
    [SerializeField] private Vector frictionVector;
    [SerializeField] private Slider staticFrictionSlider;
    [SerializeField] private Slider kineticFrictionSlider;

    [Header("Timer")]
    [SerializeField] private RectTransform timerToggle;

    [Header("Infinite distance measure")]
    [SerializeField] private InfiniteTimeLine infiniteTimeLine;
    [SerializeField] private InfiniteIceDot infiniteDot;



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
        mainObject.Find("Mesh").transform.localRotation = Quaternion.Euler(currentAffordances.physicalObject.initialRotation.ToVector3());

        Vector3 velocity = currentAffordances.physicalObject.initialVelocity.ToVector3();
        velocity.y = 0;
        velocity.z = 0;
        boxVelocity.Value = currentAffordances.physicalObject.initialVelocity.ToVector3();
        boxVelocityVector.gameObject.SetActive(currentAffordances.physicalObject.showVelocityVector);
        boxVelocityVector.SetInteractable(currentAffordances.physicalObject.velocityVectorIsInteractive);
        boxVelocityVector.Redraw();
        boxVelocityLabel.SetActive(currentAffordances.physicalObject.showVelocityLabel);

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

        pushForceToggle.SetToggle(currentAffordances.pushForce.isActive);

        // ============= Friction =============

        // TODO:
        staticFrictionCoeff.Value = 0.2f;
        staticFrictionSlider.SetValueWithoutNotify(0.2f);
        frictionVector.components.Value = Vector3.zero;
        frictionVector.Redraw();

        kineticFrictionCoeff.Value = 0.2f;
        kineticFrictionSlider.SetValueWithoutNotify(0.2f);
        
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

        mainCamera.transform.localRotation = Quaternion.Euler(currentAffordances.camera.rotation.ToVector3());

        cameraControls.gameObject.SetActive(currentAffordances.camera.showCameraControl);

        // ============= Path Renderer =============
        showPath.Value = currentAffordances.physicalObject.showTrace;
        showPathToggle.gameObject.SetActive(currentAffordances.physicalObject.showTraceIsInteractive);
        showPathToggle.GetComponent<ToggleIcons>().SetWithoutRaising(currentAffordances.physicalObject.showTrace);

        // ============= Timer Toggle =============
        timerToggle.gameObject.SetActive(true);
        timerToggle.GetComponent<ToggleIcons>().SetToFalse();
        infiniteTimeLine.InitTimeLine();

        // ============= Infinite Dot =============
        infiniteDot.InitMarkerDot();
    }
}
