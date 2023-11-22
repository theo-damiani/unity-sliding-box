using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionObject : MonoBehaviour
{
    [SerializeField] private Material material;
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
    private Vector2 uvOffset = Vector2.zero;
    private float rendererLength;
    private float uVcount;

    void Start()
    {
        material.SetVector("_UV", Vector2.zero);
        SetMaterialFromDragValue();

        // parentRigidbody = GetComponentInParent<Rigidbody>();
        parentRigidbody = targetSpeed;
        transform.position = targetPos.position + Vector3.down*targetPos.localScale.x;

        uvOffset = material.GetVector("_UV");

        Renderer renderer = GetComponent<Renderer>();
        rendererLength = renderer.bounds.size.x;
        uVcount = material.GetVector("_Tiling").x;
    }

    void FixedUpdate()
    {
        if (mainCamera.isLockedOnTarget)
        {
            transform.position = targetPos.position + Vector3.down*0.5f;
            float speedUV = parentRigidbody.velocity.x  * uVcount / rendererLength;
            uvOffset += new Vector2(-speedUV*Time.fixedDeltaTime, 0);
            material.SetVector("_UV", uvOffset);
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
