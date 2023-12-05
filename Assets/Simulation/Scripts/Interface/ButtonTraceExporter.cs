using UnityEngine;
using UnityEngine.UI;

public class ButtonTraceExporter : AnalyticsExporter
{
    [SerializeField] private Button button;
    [SerializeField] private bool buttonIsOnlyClickable;

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

    public override void CreatAndSendNewTrace()
    {
        UserTraceHolder newUserTrace = new(Time.timeSinceLevelLoad.ToString("F2"), button.gameObject.name, !buttonIsOnlyClickable, "");

        SendNewTrace(newUserTrace);
    }
}
