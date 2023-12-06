using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DraggableVectorTraceExporter : AnalyticsExporter
{
    [SerializeField] private DraggableVector vector;

    void OnEnable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown += WrapperPressTrace;
            vector.GetHeadClickZone().OnZoneMouseUp += WrapperReleaseTrace;
        }
    } 

    void OnDisable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown -= WrapperPressTrace;
            vector.GetHeadClickZone().OnZoneMouseUp -= WrapperReleaseTrace;
        }
    }

    private void WrapperPressTrace(VectorClickZone clickZone)
    {
        CreatAndSendNewTrace(UnityActionType.Press);
    }
    private void WrapperReleaseTrace(VectorClickZone clickZone)
    {
        CreatAndSendNewTrace(UnityActionType.Release);
    }
    private void CreatAndSendNewTrace(UnityActionType actionType)
    {
        UserTraceHolder newUserTrace = new(Time.timeSinceLevelLoadAsDouble, vector.gameObject.name, actionType, "components: " + vector.components.Value.ToString());

        SendNewTrace(newUserTrace);
    }
}
