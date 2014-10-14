using System;
using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
    public float PixelsPerUnit = 64f;
    public int PixelScale = 2;

    public float OscillationSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / (2f * PixelsPerUnit * PixelScale);
    }

    void Update()
    {
        float r = 0.3f*Mathf.Sin(Time.time*OscillationSpeed) + 0.7f;
        float g = 0.3f*Mathf.Sin(Time.time*OscillationSpeed*1.5f) + 0.7f;
        float b = 0.3f*Mathf.Sin(Time.time*OscillationSpeed*2f) + 0.7f;
        Camera.main.backgroundColor = new Color(r, g, b);
    }
}
