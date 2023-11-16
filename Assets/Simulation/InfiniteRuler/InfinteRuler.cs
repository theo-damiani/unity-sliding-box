using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinteRuler : MonoBehaviour
{
    [SerializeField] private Material rulerMaterial;
    [SerializeField] private Vector3Reference UVoffset;
    [SerializeField] private float scaleFactor;
    private Rigidbody parentRigidbody;

    void Start()
    {
        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        rulerMaterial.mainTextureOffset += new Vector2(-parentRigidbody.velocity.x*Time.deltaTime*scaleFactor, 0);
    }
}
