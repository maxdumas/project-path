using UnityEngine;
using System.Collections;

public class ScreenFadeIn : MonoBehaviour
{
    private static Texture2D overlay = null;

    public float TransitionTime;
    
    private float t;
    
    void Start()
    {
        if (overlay == null)
        {
            overlay = new Texture2D(1, 1);
            overlay.SetPixel(0, 0, Color.white);
            overlay.Apply();
        }
    }
    	
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
    }

    void OnGUI()
    {
        if (TransitionTime <= 0)
            return;
        
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f - t / TransitionTime);
        
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), overlay);
    }
}
