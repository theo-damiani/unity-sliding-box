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

    [Header("Meta Controls")]
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform resetButton;
    [SerializeField] private RectTransform metaPanel;
    [SerializeField] private BoolVariable isResetBtnEnable;

    [Header("Main object variables")]
    [SerializeField] private Transform mainObject;
    [SerializeField] private RectTransform showPathToggle;
    [SerializeField] private BoolVariable showPath;
    [SerializeField] private Vector3Variable boxVelocity;
    [SerializeField] private DraggableVector boxVelocityVector;
    [SerializeField] private GameObject boxVelocityLabel;
    [SerializeField] private BoolVariable isVelocityEquationEnable;

    [Header("Force Pushing Object 1")]
    [SerializeField] private BoolVariable pushIsActive;
    [SerializeField] private BoolVariable pushIsInteractive;
    [SerializeField] private Vector3Variable pushForce;
    [SerializeField] private BoolVariable pushShowVector;
    [SerializeField] private GameObject pushShowLabel;
    [SerializeField] private DraggableVector pushVector;
    [SerializeField] private ToggleIcons pushForceToggle;
    [SerializeField] private BoolVariable isPushEquationEnable;


    [Header("Friction")]
    [SerializeField] private FloatVariable staticFrictionCoeff;
    [SerializeField] private FloatVariable kineticFrictionCoeff;
    [SerializeField] private Vector frictionVector;
    [SerializeField] private Slider staticFrictionSlider;
    [SerializeField] private Slider kineticFrictionSlider;
    [SerializeField] private BoolVariable isFrictionEquationEnable;
    [SerializeField] private GameObject frictionLabel;


    [Header("Timer")]
    [SerializeField] private RectTransform timerToggle;

    [Header("Infinite distance measure")]
    [SerializeField] private InfiniteTimeLine infiniteTimeLine;
    [SerializeField] private InfiniteIceDot infiniteDot;

    [Header("Extra")]
    [SerializeField] private LabelPositionManager equationsManager;





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
        isResetBtnEnable.Value = currentAffordances.showResetButton;

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

        pushShowLabel.SetActive(currentAffordances.pushForce.showLabel);
        pushIsInteractive.Value = currentAffordances.pushForce.isInteractive;
        pushVector.SetInteractable(currentAffordances.pushForce.isConfigurable);

        pushForceToggle.SetToggle(currentAffordances.pushForce.isActive);

        // ============= Friction =============

        staticFrictionCoeff.Value = currentAffordances.frictionStaticCoeff;
        staticFrictionSlider.interactable = currentAffordances.frictionStaticIsInteractive;
        staticFrictionSlider.SetValueWithoutNotify(currentAffordances.frictionStaticCoeff);


        kineticFrictionCoeff.Value = currentAffordances.frictionKineticCoeff;
        staticFrictionSlider.interactable = currentAffordances.frictionKineticIsInteractive;
        kineticFrictionSlider.SetValueWithoutNotify(currentAffordances.frictionKineticCoeff);

        frictionVector.gameObject.SetActive(currentAffordances.frictionVector);
        frictionVector.components.Value = Vector3.zero;
        frictionVector.Redraw();

        if (currentAffordances.frictionVector)
        {
            frictionLabel.SetActive(currentAffordances.frictionLabel);
        }
        else
        {
            frictionLabel.SetActive(false);
        }

        
        // ============= Camera =============
        Vector3 cameraPos = currentAffordances.camera.position.ToVector3();
        cameraLockingToggle.SetWithoutRaising(currentAffordances.camera.isLockedOnObject);

        Slider zoomSlider = cameraZoomSlider.GetComponent<Slider>();
        float minDistanceToObject = (mainObject.localScale.x + mainObject.localScale.y + mainObject.localScale.z)/3;
        // Init camera
        mainCamera.transform.localRotation = Quaternion.Euler(currentAffordances.camera.rotation.ToVector3());

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
        showPathToggle.GetComponent<ToggleIcons>().SetWithoutRaising(currentAffordances.physicalObject.showTrace);

        // ============= Timer Toggle =============
        timerToggle.gameObject.SetActive(true);
        timerToggle.GetComponent<ToggleIcons>().SetToFalse();
        infiniteTimeLine.InitTimeLine();

        // ============= Infinite Dot =============
        infiniteDot.InitMarkerDot();

        // ============= Equations =============
        isFrictionEquationEnable.Value = currentAffordances.frictionEquation;
        isPushEquationEnable.Value = currentAffordances.pushForce.showEquation;
        isVelocityEquationEnable.Value = currentAffordances.physicalObject.showVelocityEquation;

        equationsManager.Init();

        // ============= UI Canvas position =============
        if (!currentAffordances.showPlayPauseButton && !currentAffordances.showResetButton)
        {
            metaPanel.gameObject.SetActive(false);
            cameraControls.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -25);
        }
        else
        {
            metaPanel.gameObject.SetActive(true);
            cameraControls.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -110);
        }

        boxVelocityLabel.GetComponent<VectorLabel>().UpdateSprite();
        pushShowLabel.GetComponent<VectorLabel>().UpdateSprite();
        frictionLabel.GetComponent<VectorLabel>().UpdateSprite();
    }
}
