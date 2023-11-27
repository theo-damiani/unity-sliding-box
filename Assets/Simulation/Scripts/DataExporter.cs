using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

[Serializable]
public readonly struct DataHolder
{
    public DataHolder(float t, Vector3 p, Vector3 r, Vector3 v)
    {
        Time = t;
        Position = p;
        Rotation = r;
        Velocity = v;
    }
    public float Time {get; init;}
    public Vector3 Position {get; init;}
    public Vector3 Rotation {get; init;}
    public Vector3 Velocity {get; init;}

    public override string ToString()
    {
        return "Time: " + Time + " | Position: " + Position + " | Rotation: " + Rotation + " | Velocity: " + Velocity;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class DataExporter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameObjectDataRecordingDone (string dataJSON);

    [SerializeField] private float timeRate;
    [SerializeField] private int maxSize;
    [SerializeField] private bool savePosition;
    [SerializeField] private bool saveRotation;
    [SerializeField] private bool saveVelocity;

    private readonly float defaultTimeRate = 0.1f;
    private List<DataHolder> dataList;
    private float timeAtStart;
    private bool isRecording;

    void Start()
    {
        dataList = new List<DataHolder>();

        // From InvokeRepeating: throw new UnityException("Invoke repeat rate has to be larger than 0.00001F)");
        timeRate = (timeRate < 0.00001F) ? defaultTimeRate : timeRate;
        isRecording = false;
    }

    public void StartGameObjectRecording()
    {
        isRecording = true;
        timeAtStart = Time.realtimeSinceStartup;
        dataList.Clear();
        InvokeRepeating(nameof(SaveData), 0f, timeRate);
    }

    public void StopGameObjectRecording()
    {
        isRecording = false;
        CancelInvoke(nameof(SaveData));

        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            GameObjectDataRecordingDone(JsonUtility.ToJson(dataList));
        #endif

        dataList.Clear();
    }

    void SaveData()
    {
        if (dataList.Count <= maxSize)
        {
            DataHolder data = new(
                    Time.realtimeSinceStartup - timeAtStart,
                    transform.position,
                    transform.rotation.eulerAngles,
                    GetComponent<Rigidbody>().velocity
                );

            dataList.Add(data);
        }
        else
        {
            StopGameObjectRecording();
            Debug.Log("Recording stopped: ");
        }
    }
}
