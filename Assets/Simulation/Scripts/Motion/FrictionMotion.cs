using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Vector3Reference currentMaxFriction;
    private Vector3 appliedForceNorm;
    private float previousVelocitySign;

    public override void InitMotion(Rigidbody rigidbody)
    {
        appliedForceNorm = appliedForceOnObject.Value.normalized;
        previousVelocitySign = Sign(rigidbody.velocity.x);
    }
    public override void ApplyMotion(Rigidbody rigidbody)
    {
        if (previousVelocitySign!=0)
        {
            if (Sign(rigidbody.velocity.x) != previousVelocitySign)
            {
                if (!rigidbody.isKinematic)
                    rigidbody.velocity = Vector3.zero;
            }
        }

        if (rigidbody.velocity.sqrMagnitude == 0f)
        {
            Vector3 staticFrictionForce = frictionStaticCoeff.Value * objectMass.Value * Physics.gravity.y * appliedForceNorm;
            if (appliedForceIsActive.Value)
            {
                if (appliedForceOnObject.Value.sqrMagnitude <= staticFrictionForce.sqrMagnitude)
                {
                    // STATIC FRICTION
                    rigidbody.AddForce(-appliedForceOnObject.Value*objectMass.Value, ForceMode.Force);
                    SetVectorRepresentation(currentFriction, -appliedForceOnObject.Value*objectMass.Value);
                    SetVectorRepresentation(currentMaxFriction, staticFrictionForce);
                    SetFrictionFlag(false);
                }
            }
            else
            {
                SetVectorRepresentation(currentFriction, Vector3.zero);
                SetVectorRepresentation(currentMaxFriction, staticFrictionForce);
                SetFrictionFlag(false);
            }
        }
        else
        {
            //Vector3 kineticFrictionForce = frictionKineticCoeff.Value * objectMass.Value * Physics.gravity.y * appliedForceNorm;
            Vector3 kineticFrictionForce = frictionKineticCoeff.Value * objectMass.Value * Physics.gravity.y * rigidbody.velocity.normalized;
            // KINETIC FRICTION
            rigidbody.AddForce(kineticFrictionForce, ForceMode.Force);
            SetVectorRepresentation(currentFriction, kineticFrictionForce);
            SetVectorRepresentation(currentMaxFriction, kineticFrictionForce);
            SetFrictionFlag(true);
        }

        previousVelocitySign = Sign(rigidbody.velocity.x);
    }

    public void SetVectorRepresentation(Vector3Reference vectorRef, Vector3 newComponents)
    {
        if (vectorRef.Value == newComponents)
        {
            return;
        }
        vectorRef.Value = newComponents;
    }

    public void SetFrictionFlag(bool newVal)
    {
        if (currentFrictionCoeff.Value == newVal)
        {
            return;
        }
        currentFrictionCoeff.Value = newVal;
    }

    private float Sign(float number) 
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
}
