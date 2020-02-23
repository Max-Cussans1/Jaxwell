using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper
{
    public static void Log(string output)
    {
        Debug.Log("Frame: " + Time.frameCount + " " + output);
    }

    public static void DrawBox(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Color color)
    {
        Debug.DrawLine(bottomLeft, bottomRight, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
        Debug.DrawLine(bottomRight, topRight, color);
        Debug.DrawLine(topLeft, topRight, color);
    }
}
