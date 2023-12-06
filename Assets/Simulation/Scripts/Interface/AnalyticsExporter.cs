using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public enum UnityActionType {
    Click,
    Press,
    Release,
}

[Serializable]
public struct UserTraceHolder
{
    public UserTraceHolder(double t, string i, UnityActionType a, string x)
    {
        time = Math.Round(t, 2);
        objectId = i;
        actionType = a;
        extra = x;
    }
    public double time;
    public string objectId;
    public UnityActionType actionType;
    public string extra;
}

public abstract class AnalyticsExporter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void NewUnityUserTrace (string dataJSON);
    public void SendNewTrace(UserTraceHolder trace)
    {
        Debug.Log(JsonUtility.ToJson(trace));
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            NewUnityUserTrace(JsonUtility.ToJson(trace));
        #endif
    }    
}
