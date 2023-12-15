using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLabel : MonoBehaviour
{
    [SerializeField] private Vector vector;
    [SerializeField] private float offset;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Vector3Variable vector3Variable;

    void OnEnable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise += UpdateSprite;
    }

    void OnDisable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise -= UpdateSprite;
    }

    public void SetSpriteOrientation()
    {
        // Vector3 cameraFarAway = mainCamera.position*1000;
        // // if (cameraFarAway.sqrMagnitude > 1000000)
        // // {
        // //     cameraFarAway = mainCamera.position;
        // // }
        // Vector3 distCameraFarAway = transform.position - cameraFarAway;
        // transform.rotation = Quaternion.LookRotation(distCameraFarAway);

        transform.rotation = mainCamera.rotation;
    }

    void UpdateSprite()
    {
        transform.localPosition = vector.components.Value + vector.components.Value.normalized * offset;
        SetSpriteOrientation();
    }
}
