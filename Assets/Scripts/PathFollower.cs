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

    private readonly Queue<Vector3> _path = new Queue<Vector3>();
    private FollowingState _state = FollowingState.Idle;
    private GameObject _lastSelection = null;

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR // We only want to compile this code when we are in the editor as it's just displaying debug stuff
        if(_path.Count != 0)
            using (var e = _path.GetEnumerator())
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
                _state = FollowingState.CreatingPath;
                _path.Clear();
            }
        }
        else if (Input.GetMouseButtonUp(0)) _state = FollowingState.Idle;

        if(Input.GetKeyUp(KeyCode.Space)) _state = FollowingState.FollowingPath;

        switch (_state)
        {
            case FollowingState.CreatingPath:
                var selection = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
                if (selection.transform == null) _state = FollowingState.Idle; 
                else if(selection.transform.gameObject.tag == "Piece" && // Only pick pieces
                    selection.transform.gameObject != _lastSelection) // And don't reselect the last piece we selected
                {
                    _path.Enqueue(selection.transform.position);
                    _lastSelection = selection.transform.gameObject;
                    Debug.Log("Added new selection to path. There are now " + _path.Count + " nodes in the path.");
                }
                break;
            case FollowingState.FollowingPath:
                if (_path.Count == 0) _state = FollowingState.Idle;
                else if (_path.Count > MaxMoves)
                {
                    Debug.LogWarning("This path is longer than the maximum allowed. Path is " + _path.Count + " units long while maximum is " + MaxMoves + ".");
                    _state = FollowingState.Idle;
                }
                else
                {
                    Vector3 target = _path.Peek();
                    Vector3 difference = target - transform.position;
                    if (difference.sqrMagnitude > PositionTolerance)
                    {
                        float angle = Vector3.Angle(transform.up, difference);
                        Quaternion targetRotation = Quaternion.LookRotation(difference, Vector3.back);
                        targetRotation.x = targetRotation.y = 0f;
                        if (Mathf.Abs(angle) > RotationTolerance)
                        {
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                                TurnSpeed*Time.deltaTime);
                            Debug.DrawRay(transform.position, transform.up, Color.green);
                        }
                        else
                            transform.position = Vector3.MoveTowards(transform.position, target,
                                MoveSpeed*Time.deltaTime);
                    }
                    else _path.Dequeue();
                }
                break;
        }
    }

    private enum FollowingState
    {
        Idle, CreatingPath, FollowingPath
    }
}
