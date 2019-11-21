using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Elements
{
    //change to the element we want to start with in editor
    public elements element = 0;
    elements platformElement;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            element = elements.fire;
            Debug.Log("Enabled Fire");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            element = elements.water;
            Debug.Log("Enabled Water");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            element = elements.earth;
            Debug.Log("Enabled Earth");
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            element = elements.air;
            Debug.Log("Enabled Air");
        }
    }
}
