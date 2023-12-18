using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyCodeListener : MonoBehaviour
{
    public BoolReference isActive;
    public KeyCode key;
    public UnityEvent responseOnKeyDown;
    public UnityEvent responseOnKeyUp;

    // Update is called once per frame
    void Update()
    {
        if (!isActive.Value)
        {
            return;
        }

        if (Input.GetKeyDown(key))
        {
            responseOnKeyDown.Invoke();
        }
        else if (Input.GetKeyUp(key))
        {
            responseOnKeyUp.Invoke();
        }
    }
}
