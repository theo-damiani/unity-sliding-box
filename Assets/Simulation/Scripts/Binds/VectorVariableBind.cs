using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorVariableBind : MonoBehaviour
{
    [SerializeField] Vector vector;
    [SerializeField] Vector3Variable vector3Variable;

    void OnEnable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        gameEvent.OnRaise += vector.Redraw;
    }

    void OnDisable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        gameEvent.OnRaise -= vector.Redraw;
    }
}
