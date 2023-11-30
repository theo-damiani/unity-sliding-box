using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableVectorTraceExporter : AnalyticsExporter
{
    [SerializeField] private DraggableVector vector;

    void OnEnable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown += WrapperCreateNewTrace;
            vector.GetHeadClickZone().OnZoneMouseUp += WrapperCreateNewTrace;
        }
    } 

    void OnDisable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown -= WrapperCreateNewTrace;
            vector.GetHeadClickZone().OnZoneMouseUp -= WrapperCreateNewTrace;
        }
    }

    private void WrapperCreateNewTrace(VectorClickZone clickZone)
    {
        CreatAndSendNewTrace();
    }
    public override void CreatAndSendNewTrace()
    {
        UserTraceHolder newUserTrace = new(Time.timeSinceLevelLoad, vector.gameObject.name, true, "components: " + vector.components.Value.ToString());

        SendNewTrace(newUserTrace);
    }
}
