using System.Collections.Generic;
using System.IO;
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
    public bool AllowContinue = false;

    private const float PositionTolerance = 0.001f;
    private const float RotationTolerance = 0.1f;

    private readonly Queue<Transform> _path = new Queue<Transform>();
    public FollowingState _state = FollowingState.Idle;
    private GameObject _lastSelection = null;
    private Vector3? _target = null;

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
                    var a = e.Current.position;
                    if (!e.MoveNext()) break;
                    var b = e.Current.position;
                    Debug.DrawLine(a, b, Color.white);
                }
            }
#endif

        switch (_state)
        {
            case FollowingState.CreatingPath:
                HandleCreatingPath();
                break;
            case FollowingState.FollowingPath:
                HandleFollowingPath();
                break;
            case FollowingState.Idle:
                HandleIdle();
                break;
        }
    }

    private void HandleCreatingPath()
    {
        if (Input.GetMouseButtonUp(0)) _state = FollowingState.Idle;

        var selection = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
        if (selection.transform == null) _state = FollowingState.Idle;
        else if (selection.transform.gameObject.tag == "Piece" && // Only pick pieces
                 selection.transform.gameObject != _lastSelection)
        // And don't reselect the last piece we selected
        {
            selection.transform.gameObject.renderer.material.color = Color.blue;
            _path.Enqueue(selection.transform);
            _lastSelection = selection.transform.gameObject;
            Debug.Log("Added new selection to path. There are now " + _path.Count + " nodes in the path.");
        }
    }

    private void HandleFollowingPath()
    {
        if (_target == null && _path.Count <= 0)
        {
            _state = FollowingState.Idle;
            AllowContinue = false;
        }
        else if (_path.Count > MaxMoves)
        {
            Debug.LogWarning("This path is longer than the maximum allowed. Path is " + _path.Count +
                             " units long while maximum is " + MaxMoves + ".");
            _state = FollowingState.Idle;
            AllowContinue = false;
        }
        else
        {
            // If we have a target, find the difference between target and current position
            // If no target is set, our difference is zero.
            Vector3 difference = (_target.HasValue) ? _target.Value - transform.position : Vector3.zero;
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
                    transform.position = Vector3.MoveTowards(transform.position, _target.Value,
                        MoveSpeed*Time.deltaTime);
            }
            else
            {
                if (AllowContinue && _path.Count >= 1) _target = _path.Dequeue().position;
                else _target = null;
            }
        }
    }

    private void HandleIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Only continue if mouse is inside the collider of the follower
            if (collider2D.OverlapPoint(mouseWorldCoords))
            {
                AllowContinue = false;
                _state = FollowingState.CreatingPath;

                // Reset old path's colors
                foreach (Transform t in _path)
                    t.gameObject.renderer.material.color = Color.white;

                _path.Clear();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _state = FollowingState.FollowingPath;
            AllowContinue = true;
        }
        else if (_path.Count > 0 && AllowContinue) _state = FollowingState.FollowingPath;
    }

    public enum FollowingState
    {
        Idle, CreatingPath, FollowingPath
    }
}
