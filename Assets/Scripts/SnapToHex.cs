using UnityEngine;

public class SnapToHex : MonoBehaviour {
    private const float Sqrt3 = 1.732050807568877293527f;
    public float PieceRadius;

	// Use this for initialization
	void Start ()
	{
	    float x = transform.position.x, y = transform.position.y;

        int r = Mathf.RoundToInt((Sqrt3 * x - y) / (3f * PieceRadius));
        int g = Mathf.RoundToInt((-Sqrt3 * x - y) / (3f * PieceRadius));
        int b = -(r + g);

	    transform.position = new Vector3(
            Sqrt3*PieceRadius*(b/2f + r),
	        3f/2f*PieceRadius*b,
	        transform.position.z
	        );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
