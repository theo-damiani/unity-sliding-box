using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VectorLabel : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
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

    private void SetSpriteOrientation()
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

    public void UpdateSprite()
    {
        if (vector3Variable.Value == Vector3.zero)
        {
            sprite.SetActive(false);
            return;
        }
        sprite.SetActive(true);
        transform.localPosition = vector3Variable.Value + vector3Variable.Value.normalized * offset;
        SetSpriteOrientation();
    }
}
