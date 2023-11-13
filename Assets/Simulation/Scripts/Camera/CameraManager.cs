using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float minZ;
    public float maxZ;
    [SerializeField] private Slider zoomSlider;
    [HideInInspector] public bool isLockedOnTarget = true;
    private Vector3 initCameraPos;
    private Vector3 initOffsetToTarget;
    private Vector3 distanceToTarget;

    void Start()
    {
        
    }

    public void InitCamera(Vector3 initPos, bool isLocked)
    {
        // Cache the initial offset at time of load/spawn:
        initCameraPos = initPos;
        gameObject.transform.localPosition = initPos;
        isLockedOnTarget = isLocked;
        SetOffsetToTarget();
    }

    public void SetOffsetToTarget()
    {
        initOffsetToTarget = gameObject.transform.localPosition - target.localPosition;
        distanceToTarget = gameObject.transform.localPosition - target.localPosition;
    }

    void LateUpdate()
    {
        if (isLockedOnTarget)
        {
            gameObject.transform.localPosition = target.localPosition + distanceToTarget;
        }
    }

    public void ZoomAlongZ(float value)
    {
        if (isLockedOnTarget) 
        {
            distanceToTarget = new Vector3(distanceToTarget.x, distanceToTarget.y, SliderToCameraZ(value));
        }
        else
        {
            Vector3 currentCamPos = gameObject.transform.localPosition;
            gameObject.transform.localPosition = new Vector3(currentCamPos.x, currentCamPos.y, SliderToCameraZ(value));
        }
    }

    public void ZoomInAlongZ()
    {
        if (distanceToTarget.z+1 <= minZ)
        {
            distanceToTarget += Vector3.forward;
        }
    }

    public void ZoomOutAlongZ()
    {
        if (distanceToTarget.z-1 >= maxZ)
        {
            distanceToTarget -= Vector3.forward;
        }
    }

    public void ToggleCameraLocked()
    {
        isLockedOnTarget = !isLockedOnTarget;
        if (isLockedOnTarget)
        {
            zoomSlider.SetValueWithoutNotify(CameraToSliderZ(initCameraPos.z));
            distanceToTarget = initOffsetToTarget;
        }
    }

    public void SetCameraLocked(bool isLocked)
    {
        isLockedOnTarget = isLocked;
        Debug.Log(isLockedOnTarget);
        if (isLockedOnTarget)
        {
            zoomSlider.SetValueWithoutNotify(CameraToSliderZ(initCameraPos.z));
            distanceToTarget = initOffsetToTarget;
        }
    }
    
    public float GetSliderMinZ()
    {
        if (minZ<0 && maxZ<0)
            return CameraToSliderZ(maxZ);
        else
            return CameraToSliderZ(minZ);
    }

    public float GetSliderMaxZ()
    {
        if (minZ<0 && maxZ<0)
            return CameraToSliderZ(minZ);
        else
            return CameraToSliderZ(maxZ);
    }

    public float CameraToSliderZ(float value)
    {
        return Mathf.Log(-value);
    }

    public float SliderToCameraZ(float value)
    {
        return -Mathf.Exp(value);
    }
}
