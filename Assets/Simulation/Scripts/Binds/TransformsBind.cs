using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransformsBinds : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Update()
    {
        Vector3 newPosition = target.localPosition;
        newPosition.y = transform.localPosition.y;
        transform.localPosition = newPosition;
    }
}
