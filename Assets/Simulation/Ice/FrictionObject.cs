using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionObject : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float scaleFactor;
    [SerializeField] private FloatReference drag;

    [Header("Position configuration")]
    [SerializeField] private Transform targetPos;
    [SerializeField] private Rigidbody targetSpeed;
    [SerializeField] private CameraManager mainCamera;

    [Header("Material Ice Configuration")]
    [SerializeField] private float metallicOnIce;
    [SerializeField] private float smoothnessOnIce;
    [SerializeField] private float refractionOffsetOnIce;
    [SerializeField] private float refractionScaleOnIce;
    [SerializeField] private float refractionOnIce;
    [SerializeField] private float normalsOnIce;
    [ColorUsage(true, true), SerializeField] private Color colorOnIce;

    [Header("Material Grip Configuration")]
    [SerializeField] private float metallicOnGrip;
    [SerializeField] private float smoothnessOnGrip;
    [SerializeField] private float refractionOffsetOnGrip;
    [SerializeField] private float refractionScaleOnGrip;
    [SerializeField] private float refractionOnGrip;
    [SerializeField] private float normalsOnGrip;
    [ColorUsage(true, true), SerializeField] private Color colorOnGrip;

    [Header("Easing")]
    [SerializeField] AnimationCurve curve;

    private Rigidbody parentRigidbody;

    void Start()
    {
        material.SetVector("_UV", Vector2.zero);
        SetMaterialFromDragValue();

        // parentRigidbody = GetComponentInParent<Rigidbody>();
        parentRigidbody = targetSpeed;
        transform.position = targetPos.position + Vector3.down*0.5f;
    }

    void Update()
    {
        if (mainCamera.isLockedOnTarget)
        {
            transform.position = targetPos.position + Vector3.down*0.5f;
            Vector2 oldUV = material.GetVector("_UV");
            material.SetVector("_UV", oldUV + new Vector2(-parentRigidbody.velocity.x*Time.deltaTime*scaleFactor, 0));
        }
    }

    void OnEnable()
    {
        GameEvent gameEvent = drag.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise += SetMaterialFromDragValue;
    }

    void OnDisable()
    {
        GameEvent gameEvent = drag.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise -= SetMaterialFromDragValue;
    }

    public void SetMaterialFromDragValue()
    {
        float lerpTime = drag.Value * 1;
        float easingTime = curve.Evaluate(lerpTime);

        material.SetFloat("_Metallic", Mathf.Lerp(metallicOnIce, metallicOnGrip, lerpTime));
        material.SetFloat("_Smoothness", Mathf.Lerp(smoothnessOnIce, smoothnessOnGrip, lerpTime));
        material.SetFloat("_Normals", Mathf.Lerp(normalsOnIce, normalsOnGrip, lerpTime));
        material.SetFloat("_Refraction", Mathf.Lerp(refractionOnIce, refractionOnGrip, easingTime));
        material.SetFloat("_RefractionScale", Mathf.Lerp(refractionScaleOnIce, refractionScaleOnGrip, lerpTime));
        material.SetFloat("_RefractionOffset", Mathf.Lerp(refractionOffsetOnIce, refractionOffsetOnGrip, easingTime));
        material.SetColor("_Color", Color.Lerp(colorOnIce, colorOnGrip, easingTime));
    }
}
