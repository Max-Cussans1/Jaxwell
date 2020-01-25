using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper
{
    public static void Log(string output)
    {
        Debug.Log("Frame: " + Time.frameCount + " " + output);
    }
}
