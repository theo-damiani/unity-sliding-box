using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IceObject : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float scaleFactor;
    [SerializeField] private FloatReference drag;

    [Header("Material Ice Configuration")]
    [SerializeField] private float metallicOnIce;
    [SerializeField] private float smoothnessOnIce;
    [SerializeField] private float normalsOnIce;
    [SerializeField] private float refractionOnIce;
    [SerializeField] private float frostPowerOnIce;
    [ColorUsage(true, true), SerializeField] private Color mainColorOnIce;
    [ColorUsage(true, true), SerializeField] private Color frostColorOnIce;

    [Header("Material Grip Configuration")]
    [SerializeField] private float metallicOnGrip;
    [SerializeField] private float smoothnessOnGrip;
    [SerializeField] private float normalsOnGrip;
    [SerializeField] private float refractionOnGrip;
    [SerializeField] private float frostPowerOnGrip;
    [ColorUsage(true, true), SerializeField] private Color mainColorOnGrip;
    [ColorUsage(true, true), SerializeField] private Color frostColorOnGrip;

    private Rigidbody parentRigidbody;

    void Start()
    {
        material.SetVector("_UV", Vector2.zero);
        SetMaterialFromDragValue();

        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        Vector2 oldUV = material.GetVector("_UV");
        material.SetVector("_UV", oldUV + new Vector2(-parentRigidbody.velocity.x*Time.deltaTime*scaleFactor, 0));
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

        material.SetFloat("_Metallic", Mathf.Lerp(metallicOnIce, metallicOnGrip, lerpTime));
        material.SetFloat("_Smoothness", Mathf.Lerp(smoothnessOnIce, smoothnessOnGrip, lerpTime));
        material.SetFloat("_Normals", Mathf.Lerp(normalsOnIce, normalsOnGrip, lerpTime));
        material.SetFloat("_Refraction", Mathf.Lerp(refractionOnIce, refractionOnGrip, lerpTime));
        material.SetColor("_MainColor", Color.Lerp(mainColorOnIce, mainColorOnGrip, lerpTime));
        //material.SetFloat("_FrostPower", Mathf.Lerp(frostPowerOnIce, frostPowerOnGrip, lerpTime));
        //material.SetColor("_FrostColor", Color.Lerp(frostColorOnIce, frostColorOnGrip, lerpTime));
    }

    public void SetIceEffect()
    {
        material.SetFloat("_Metallic", metallicOnIce);
        material.SetFloat("_Smoothness", smoothnessOnIce);
        material.SetFloat("_Normals", normalsOnIce);
        material.SetFloat("_Refraction", refractionOnIce);
        material.SetColor("_MainColor", mainColorOnIce);
        material.SetFloat("_FrostPower", frostPowerOnIce);
        material.SetColor("_FrostColor", frostColorOnIce);
    }

    public void SetGripEffect()
    {
        material.SetFloat("_Metallic", metallicOnGrip);
        material.SetFloat("_Smoothness", smoothnessOnGrip);
        material.SetFloat("_Normals", normalsOnGrip);
        material.SetFloat("_Refraction", refractionOnGrip);
        material.SetFloat("_FrostPower", frostPowerOnGrip);
        material.SetColor("_FrostColor", frostColorOnGrip);
        material.SetColor("_MainColor", mainColorOnGrip);
    }
}
