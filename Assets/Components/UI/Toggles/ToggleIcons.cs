using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleIcons : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite trueIcon;
    [SerializeField] private Color trueColor;
    [SerializeField] private Sprite falseIcon;
    [SerializeField] private Color falseColor;

    [SerializeField] private UnityEvent OnTrueEventList;
    [SerializeField] private UnityEvent OnFalseEventList;

    private bool isTrue = false;

    public void SetToTrue()
    {
        isTrue = true;
        if (icon)
        {
            icon.sprite = trueIcon;
            icon.color = trueColor;
        }

        OnTrueEventList?.Invoke();
    }

    public void SetToFalse()
    {
        isTrue = false;
        if (icon)
        {
            icon.sprite = falseIcon;
            icon.color = falseColor;
        }

        OnFalseEventList?.Invoke();
    }

    public void SetWithoutRaising(bool isTrue)
    {
        this.isTrue = isTrue;
        if (isTrue)
        {
            if (icon) 
            {
                icon.sprite = trueIcon;
                icon.color = trueColor;
            }
        }
        else
        {
            if (icon)
            {
                icon.sprite = falseIcon;
                icon.color = falseColor;
            }
        }
    }

    public void SetToggle(bool isTrue)
    {
        if (isTrue)
        {
            SetToTrue();
        }
        else
        {
            SetToFalse();
        }
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
}
