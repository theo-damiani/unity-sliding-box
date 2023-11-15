using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "pushMotion", menuName="Motion / Push Motion")]
public class PushMotion : Motion
{
    [SerializeField] private float forceScale;
    public override void InitMotion(Rigidbody rigidbody)
    {
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        rigidbody.AddForce(force.Value*forceScale, ForceMode.Force);
    }
}
