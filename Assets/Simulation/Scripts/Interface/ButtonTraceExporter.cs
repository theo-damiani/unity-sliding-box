using UnityEngine;
using UnityEngine.UI;

public class ButtonTraceExporter : AnalyticsExporter
{
    [SerializeField] private Button button;

    void OnEnable()
    {
        if(button)
        {
            button.onClick.AddListener(CreatAndSendNewTrace);
        }
    } 

    void OnDisable()
    {
        if(button)
        {
            button.onClick.RemoveListener(CreatAndSendNewTrace);
        }
    }

    private void CreatAndSendNewTrace()
    {
        UserTraceHolder newUserTrace = new(Time.timeSinceLevelLoadAsDouble, button.gameObject.name, UnityActionType.Click, "");

        SendNewTrace(newUserTrace);
    }
}
