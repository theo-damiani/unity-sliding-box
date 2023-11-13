using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIcons : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite trueIcon;
    [SerializeField] private Sprite falseIcon;
    [SerializeField] private List<GameEvent> OnTrueEventList;
    [SerializeField] private List<GameEvent> OnFalseEventList;

    private bool isTrue = false;

    public void SetToTrue()
    {
        isTrue = true;
        if (icon) icon.sprite = trueIcon;

        RaiseEventsInList(OnTrueEventList);
    }

    public void SetWithoutRaising(bool isTrue)
    {
        this.isTrue = isTrue;
        if (isTrue)
        {
            if (icon) icon.sprite = trueIcon;
        }
        else
        {
            if (icon) icon.sprite = falseIcon;
        }
    }

    public void SetToFalse()
    {
        isTrue = false;
        if (icon) icon.sprite = falseIcon;

        RaiseEventsInList(OnFalseEventList);
    }

    public void ToggleTrueFalse()
    {
        if (isTrue)
        {
            SetToFalse();
        }
        else
        {
            SetToTrue();
        }
    }

    private void RaiseEventsInList(List<GameEvent> listGameEvents)
    {
        foreach (GameEvent e in listGameEvents)
        {
            e.Raise();
        }
    }
}
