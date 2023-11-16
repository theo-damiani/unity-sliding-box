using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObject : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float scaleFactor;
    private Rigidbody parentRigidbody;

    void Start()
    {
        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        Vector2 oldUV = material.GetVector("_UV");
        material.SetVector("_UV", oldUV + new Vector2(-parentRigidbody.velocity.x*Time.deltaTime*scaleFactor, 0));
    }
}
