using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "frictionMotion", menuName="Motion / Friction Motion")]
public class FrictionMotion : Motion
{
    [SerializeField] private FloatReference frictionMaxCoeff;
    [SerializeField] private FloatReference objectMass;
    [SerializeField] private Vector3Reference appliedForceOnObject;
    [SerializeField] private BoolReference appliedForceIsActive;
    [SerializeField] private Vector3Reference currentFriction;
    private Vector3 appliedForceNorm;

    public override void InitMotion(Rigidbody rigidbody)
    {
        appliedForceNorm = appliedForceOnObject.Value.normalized;
    }
    public override void ApplyMotion(Rigidbody rigidbody)
    {
        Vector3 frictionForce = frictionMaxCoeff.Value * objectMass.Value * Physics.gravity.y * appliedForceNorm;
        if (appliedForceIsActive.Value)
        {
            if (frictionForce.sqrMagnitude >= appliedForceOnObject.Value.sqrMagnitude)
            {
                rigidbody.AddForce(-appliedForceOnObject.Value*objectMass.Value, ForceMode.Force); // static friction
                SetVectorRepresentation(-appliedForceOnObject.Value*objectMass.Value);
            }
            else
            {
                //Debug.Log("kinetic: " +  frictionForce);
                rigidbody.AddForce(frictionForce, ForceMode.Force); // kinetic friction
                SetVectorRepresentation(frictionForce);
            }
        }
        else
        {
            if (Mathf.Approximately(rigidbody.velocity.sqrMagnitude, 0))
            {
                return;
            }
            //if (Mathf.Approximately(rigidbody.velocity.sqrMagnitude, 0))
            if (rigidbody.velocity.sqrMagnitude <= 0.01)
            {
                //Debug.Log("ZERO");
                rigidbody.velocity = Vector3.zero;
                SetVectorRepresentation(Vector3.zero);
                return;
            }
            rigidbody.AddForce(frictionForce, ForceMode.Force); // kinetic friction
            SetVectorRepresentation(frictionForce);

            //float decelleration = 2*rigidbody.velocity.sqrMagnitude/(2*frictionMaxCoeff.Value*Physics.gravity.y);
            // Debug.Log(1/decelleration);
            // rigidbody.AddForce(rigidbody.velocity.normalized*1/decelleration,  ForceMode.Acceleration);
            //rigidbody.AddForce()
        }
    }

    public void SetVectorRepresentation(Vector3 newComponents)
    {
        if (currentFriction.Value == newComponents)
        {
            return;
        }
        currentFriction.Value = newComponents;
    }
}
