using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    /// <summary>
    /// How fast to move the unit, in units/s
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// How fast to turn the unit, in deg/s
    /// </summary>
    public float TurnSpeed;
    public int MaxMoves;

    private const float PositionTolerance = 0.001f;
    private const float RotationTolerance = 0.1f;
    private readonly Queue<Vector3> path = new Queue<Vector3>();
    private FollowingState state = FollowingState.Idle;
    private GameObject lastSelection = null;

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR // We only want to compile this code when we are in the editor as it's just displaying debug stuff
        if(path.Count != 0)
            using (var e = path.GetEnumerator())
            {
                e.MoveNext();
                while (true)
                {
                    var a = e.Current;
                    if (!e.MoveNext()) break;
                    var b = e.Current;
                    Debug.DrawLine(a, b, Color.white);
                }
            }
#endif

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Only continue if mouse is inside the collider of the follower
            if (collider2D.OverlapPoint(mouseWorldCoords))
            {
                state = FollowingState.CreatingPath;
                path.Clear();
            }
        }
        else if (Input.GetMouseButtonUp(0)) state = FollowingState.Idle;

        if(Input.GetKeyUp(KeyCode.Space)) state = FollowingState.FollowingPath;

        switch (state)
        {
            case FollowingState.CreatingPath:
                var selection = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
                if (selection.transform.gameObject.tag == "Piece" && // Only pick pieces
                    selection.transform.gameObject != lastSelection) // And don't reselect the last piece we selected
                {
                    path.Enqueue(selection.transform.position);
                    lastSelection = selection.transform.gameObject;
                    Debug.Log("Added new selection to path. There are now " + path.Count + " nodes in the path.");
                }
                break;
            case FollowingState.FollowingPath:
                if (path.Count == 0) state = FollowingState.Idle;
                else if (path.Count > MaxMoves)
                {
                    Debug.LogWarning("This path is longer than the maximum allowed. Path is " + path.Count + " units long while maximum is " + MaxMoves + ".");
                    state = FollowingState.Idle;
                }
                else
                {
                    Vector3 target = path.Peek();
                    Vector3 difference = target - transform.position;
                    if (difference.sqrMagnitude > PositionTolerance)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(difference, Vector3.back);
                        targetRotation.x = targetRotation.y = 0f;
                        float angle = Vector3.Angle(transform.up, difference);
                        if (angle > RotationTolerance)
                        {
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                                TurnSpeed*Time.deltaTime);
                            Debug.DrawRay(transform.position, transform.up, Color.green);
                        }
                        else
                            transform.position = Vector3.MoveTowards(transform.position, target,
                                MoveSpeed*Time.deltaTime);
                    }
                    else path.Dequeue();
                }
                break;
        }
    }

    private enum FollowingState
    {
        Idle, CreatingPath, FollowingPath
    }
}
