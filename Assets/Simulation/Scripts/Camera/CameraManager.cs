using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float maxCameraDist;
    public bool isLockedOnTarget = true;
    public Slider zoomSlider;

    private Vector3 initOffsetToTarget;
    private Vector3 distanceToTarget;
    private Vector3 minDistanceToTarget;
    private Vector3 zoomDirScaled = Vector3.zero;
    private Vector3 previousTargetPos;

    void Start()
    {
        if (!target)
        {
            GameObject newTarget = new("Camera Target");
            newTarget.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            target = newTarget.transform;
            InitCamera(target, Vector3.back*5, false, 0.1f, zoomSlider);
        }
    }

    public void InitCamera(Transform target, Vector3 initPos, bool isLocked, float minDistanceToObject, Slider uiSlider)
    {
        this.target = target;

        // min/max value config at the top, because it will change slider.value and so call the function ZoomInOutTarget!
        uiSlider.minValue = 1;
        uiSlider.maxValue = GetSliderMax();

        // Set initial camera pos
        transform.localPosition = initPos;
        previousTargetPos = target.localPosition;
        isLockedOnTarget = isLocked;
        // Check if initial pos is in bounds
        initOffsetToTarget = initPos - previousTargetPos;
        float initOffsetClamped = Mathf.Clamp(initOffsetToTarget.magnitude, minDistanceToObject, GetSliderMax());
        initOffsetToTarget = initOffsetToTarget.normalized*initOffsetClamped;

        distanceToTarget = initOffsetToTarget;
        minDistanceToTarget = (initPos - target.localPosition).normalized * minDistanceToObject;
        uiSlider.SetValueWithoutNotify(CameraToSlider(initOffsetToTarget.magnitude));
        zoomDirScaled = minDistanceToTarget * initOffsetToTarget.magnitude;

        zoomSlider = uiSlider;
    }

    void LateUpdate()
    {
        if (isLockedOnTarget)
        {
            gameObject.transform.localPosition = target.localPosition + distanceToTarget;
            previousTargetPos = target.localPosition;
        }
    }

    public void ZoomInOutTarget(float value)
    {
        zoomDirScaled = minDistanceToTarget * SliderToCamera(value);
        if (isLockedOnTarget)
        {
            distanceToTarget = zoomDirScaled;
        }
        else
        {
            gameObject.transform.localPosition = previousTargetPos + zoomDirScaled;
        }
    }

    public void ToggleCameraLocked()
    {
        isLockedOnTarget = !isLockedOnTarget;
        if (!isLockedOnTarget)
        {
            // from Locked to UnLocked
            gameObject.transform.localPosition = target.localPosition + zoomDirScaled;
        }
        else
        {
            distanceToTarget = zoomDirScaled;
        }
    }

    public void SetCameraLocked(bool isLocked)
    {
        isLockedOnTarget = isLocked;
        if (!isLockedOnTarget)
        {
            // from Locked to UnLocked
            gameObject.transform.localPosition = target.localPosition + zoomDirScaled;
        }
        else
        {
            distanceToTarget = zoomDirScaled;
        }
    }

    public float GetSliderMax()
    {
            return CameraToSlider(maxCameraDist);
    }

    public float CameraToSlider(float value)
    {
        // return Mathf.Log(value);
        return value;
    }

    public float SliderToCamera(float value)
    {
        // return Mathf.Exp(value);
        return value;
    }
}
