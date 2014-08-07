using UnityEngine;

public class FollowPanCamera : MonoBehaviour
{
    public Transform Target = null;
    public bool FollowTarget = false;
    /// <summary>
    /// The lerp coefficient for following the target.
    /// </summary>
    public float FollowSpeed = 0.25f;
    /// <summary>
    /// Pixel-to-Unit ratio for mouse panning. (i.e. higher number means slower)
    /// </summary>
    public float PanSpeed = 100;

    public bool AtTarget { get; private set; }

    private bool _isPanning;

    // Use this for initialization
    void Start()
    {
	    
    }
	
    // Update is called once per frame
    void Update()
    {
        if (FollowTarget && Target != null) // Lazy-follow the target
        {
            AtTarget = false;
            if (Vector2.Distance(Target.position, transform.position) > 0.01f)
            {
                float x = Mathf.SmoothStep(transform.position.x, Target.transform.position.x, FollowSpeed);
                float y = Mathf.SmoothStep(transform.position.y, Target.transform.position.y, FollowSpeed);

                transform.position = new Vector3(x, y, transform.position.z);
            }
            else
                AtTarget = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            FollowTarget = false;
            _isPanning = true;
            AtTarget = false;
        }

        if (_isPanning)
        {
            if (Input.GetMouseButtonUp(1))
                _isPanning = false;
            Vector3 delta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            transform.Translate(delta / 20 * camera.orthographicSize);
        }

        if (Input.GetKeyUp(KeyCode.Space))
            FollowTarget = true;
    }
}
