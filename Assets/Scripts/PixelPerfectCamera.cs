using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
    public float PixelsPerUnit = 64f;
    public int PixelScale = 2;

    // Use this for initialization
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / (2f * PixelsPerUnit * PixelScale);
    }
}
