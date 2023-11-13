using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLabel : MonoBehaviour
{
    [SerializeField] private Vector vector;
    [SerializeField] private float offset;

    void LateUpdate()
    {
        transform.localPosition = vector.components.Value + vector.components.Value.normalized * offset;
    }
}
