using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ToggleStartActivation : MonoBehaviour
{
    [SerializeField] private Toggle toggleOn;
    [SerializeField] private Toggle toggleOff;

    public void SetToggleVisibility(bool value)
    {
        toggleOn.isOn = value;
        toggleOff.isOn = !value;
    }

    public void SetToggleVisibilityWithoutNotify(bool value)
    {
        toggleOff.SetIsOnWithoutNotify(!value);
        //toggleOff.GetComponent<Image>().raycastTarget = !value;
        toggleOn.SetIsOnWithoutNotify(value);
        //toggleOn.GetComponent<Image>().raycastTarget = value;
    }

    public void SetToggleOff()
    {
        toggleOn.isOn = false;
        toggleOff.isOn = true;
    }
}
