using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour
{
    public string NextLevel;

    // This is a kludge, called right before OnDestroy.
    void StartChangeLevel()
    {
        GameObject changer = new GameObject("LevelChanger");
        LevelTransition l = changer.AddComponent<LevelTransition>();
        l.Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        l.TransitionTime = 1f;
        l.FollowCam = Camera.main.GetComponent<FollowPanCamera>();
        l.NextLevel = NextLevel;
    }
}
