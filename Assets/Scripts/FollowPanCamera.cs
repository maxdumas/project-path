using UnityEngine;

public class FollowPanCamera : MonoBehaviour
{
    public GameObject Target = null;
    public bool FollowTarget = false;
    /// <summary>
    /// The lerp coefficient for following the target.
    /// </summary>
    public float FollowSpeed = 0.25f;
    /// <summary>
    /// Pixel-to-Unit ratio for mouse panning. (i.e. higher number means slower)
    /// </summary>
    public float PanSpeed = 100;

    private bool _isPanning;

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (FollowTarget && Target != null) // Lazy-follow the target
	    {
	        if (!Target.transform.position.Equals(transform.position))
	        {
	            float x = Mathf.SmoothStep(transform.position.x, Target.transform.position.x, FollowSpeed);
	            float y = Mathf.SmoothStep(transform.position.y, Target.transform.position.y, FollowSpeed);

                transform.position = new Vector3(x, y, transform.position.z);
	        }
	    }

	    if (Input.GetMouseButtonDown(1))
	    {
	        FollowTarget = false;
	        _isPanning = true;
	    }

	    if (_isPanning)
	    {
	        if (Input.GetMouseButtonUp(1)) _isPanning = false;
            Vector3 delta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
	        transform.Translate(delta / camera.orthographicSize);
	    }
	}
}
