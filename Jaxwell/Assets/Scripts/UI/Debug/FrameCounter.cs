using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    Text frameCount;

    void Start()
    {
        frameCount = GetComponent<Text>();
    }

    void Update()
    {
        frameCount.text = Time.frameCount.ToString();
    }
}
