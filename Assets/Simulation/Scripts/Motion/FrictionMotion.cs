using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "frictionMotion", menuName="Motion / Friction Motion")]
public class FrictionMotion : Motion
{
    [SerializeField] private FloatReference frictionStaticCoeff;
    [SerializeField] private FloatReference frictionKineticCoeff;
    [SerializeField] private FloatReference objectMass;
    [SerializeField] private Vector3Reference appliedForceOnObject;
    [SerializeField] private BoolReference appliedForceIsActive;
    [SerializeField] private Vector3Reference currentFriction;
    [SerializeField] private BoolReference currentFrictionCoeff; // 0 for static, 1 for kinetic
    private Vector3 appliedForceNorm;

    public override void InitMotion(Rigidbody rigidbody)
    {
        appliedForceNorm = appliedForceOnObject.Value.normalized;
    }
    public override void ApplyMotion(Rigidbody rigidbody)
    {
        Vector3 kineticFrictionForce = frictionKineticCoeff.Value * objectMass.Value * Physics.gravity.y * appliedForceNorm;
        if (appliedForceIsActive.Value)
        {
            Vector3 staticFrictionForce = frictionStaticCoeff.Value * objectMass.Value * Physics.gravity.y * appliedForceNorm;
            if (staticFrictionForce.sqrMagnitude >= appliedForceOnObject.Value.sqrMagnitude)
            {
                // // STATIC FRICTION
                // rigidbody.AddForce(-appliedForceOnObject.Value*objectMass.Value, ForceMode.Force);
                // SetVectorRepresentation(-appliedForceOnObject.Value*objectMass.Value);

                if (rigidbody.velocity.sqrMagnitude <= 0.01)
                {
                    rigidbody.velocity = Vector3.zero;
                }

                if (rigidbody.velocity != Vector3.zero)
                {
                    // KINETIC FRICTION
                    rigidbody.AddForce(kineticFrictionForce, ForceMode.Force);
                    SetVectorRepresentation(kineticFrictionForce);
                    SetFrictionFlag(true);
                }
                else
                {
                    // STATIC FRICTION
                    rigidbody.AddForce(-appliedForceOnObject.Value*objectMass.Value, ForceMode.Force);
                    SetVectorRepresentation(-appliedForceOnObject.Value*objectMass.Value);
                    SetFrictionFlag(false);
                }
            }
            else
            {
                // KINETIC FRICTION
                rigidbody.AddForce(kineticFrictionForce, ForceMode.Force);
                SetVectorRepresentation(kineticFrictionForce);
                SetFrictionFlag(true);
            }
        }
        else
        {
            if (Mathf.Approximately(rigidbody.velocity.sqrMagnitude, 0))
            {
                return;
            }
            if (rigidbody.velocity.sqrMagnitude <= 0.01)
            {
                rigidbody.velocity = Vector3.zero;
                SetVectorRepresentation(Vector3.zero);
                SetFrictionFlag(false);
                return;
            }
            // KINETIC FRICTION
            rigidbody.AddForce(kineticFrictionForce, ForceMode.Force);
            SetVectorRepresentation(kineticFrictionForce);
            SetFrictionFlag(true);
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

    public void SetFrictionFlag(bool newVal)
    {
        if (currentFrictionCoeff.Value == newVal)
        {
            return;
        }
        currentFrictionCoeff.Value = newVal;
    }
}
