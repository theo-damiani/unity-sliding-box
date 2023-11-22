using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "pushMotion", menuName="Motion / Push Motion")]
public class PushMotion : Motion
{
    [SerializeField] private Vector3Reference force;
    [SerializeField] private Vector3Reference initForce;
    [SerializeField] private FloatReference objectMass;
    public override void InitMotion(Rigidbody rigidbody)
    {
        // if (!forceIsConstant.Value)
        // {
        //     force.Value = Vector3.right;
        // }
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        rigidbody.AddForce(force.Value * objectMass.Value, ForceMode.Force); // pushing force
    }
}
