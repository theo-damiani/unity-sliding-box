using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public struct UserTraceHolder
{
    public UserTraceHolder(string t, string i, bool h, string x)
    {
        time = t;
        objectId = i;
        objectIsHold = h;
        extra = x;
    }
    public string time;
    public string objectId;
    public bool objectIsHold;
    public string extra;
}

public abstract class AnalyticsExporter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void NewUnityUserTrace (string dataJSON);

    public abstract void CreatAndSendNewTrace();
    public void SendNewTrace(UserTraceHolder trace)
    {
        Debug.Log(JsonUtility.ToJson(trace));
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            NewUnityUserTrace(JsonUtility.ToJson(trace));
        #endif
    }    
}
