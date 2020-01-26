using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    Text currentfps;
    float fps = 0f;

    void Start()
    {
        currentfps = GetComponent<Text>();
    }

    void Update()
    {
        fps = 1.0f / Time.unscaledDeltaTime;
        currentfps.text = "FPS: " + fps.ToString();
    }
}
