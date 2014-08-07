using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    private static Texture2D overlay = null; 

    public FollowPanCamera FollowCam;
    public Player Player;
    public float TransitionTime;
    public string NextLevel;

    private bool transition = false;
    private float t;

    // Use this for initialization
    void Start()
    {
        if (overlay == null)
        {
            overlay = new Texture2D(1, 1);
            overlay.SetPixel(0, 0, Color.white);
            overlay.Apply();
        }
        FollowCam.Target = Player.transform;
        FollowCam.FollowTarget = true;
    }
	
    // Update is called once per frame
    void Update()
    {
        // Wait until camera centers on player before transitioning
        if (!string.IsNullOrEmpty(NextLevel) && FollowCam.AtTarget)
            transition = true;

        if (transition)
            t += Time.deltaTime;

        if (t >= TransitionTime)
            Application.LoadLevel(NextLevel);
    }

    void OnGUI()
    {
        if (TransitionTime <= 0)
            return;

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, t / TransitionTime);

        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), overlay);
    }
}