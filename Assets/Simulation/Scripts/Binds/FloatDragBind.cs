using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatDragBind : MonoBehaviour
{
    [SerializeField] private FloatVariable dragVariable;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SetVariableFromRigidbody();
    }

    public void SetVariableFromRigidbody()
    {
        if (dragVariable.Value == rb.drag)
        {
            return;
        }
        dragVariable.Value = rb.drag;
    }
}
