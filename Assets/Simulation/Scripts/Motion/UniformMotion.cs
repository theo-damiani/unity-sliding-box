using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "uniformMotion", menuName="Motion / Uniform Motion")]
public class UniformMotion : Motion
{
    [SerializeField] private Vector3Reference force;
    public override void InitMotion(Rigidbody rigidbody)
    {
        // rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force.Value, ForceMode.VelocityChange);
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        //rigidbody.AddForce(velocity.Value, ForceMode.Force);
    }
}
