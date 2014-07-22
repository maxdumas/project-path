using UnityEngine;

public class FadeAndRise : MonoBehaviour
{
    public float Lifetime = 1f;
    public float Delay = 0f;
    public float EndYOffset;

    private float _time = 0f;
    private float _startY, _endY;

	// Use this for initialization
	void Start ()
	{
	    _startY = transform.position.y;
	    _endY = transform.position.y + EndYOffset;
        Destroy(gameObject, Delay + Lifetime);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _time += Time.deltaTime;

	    if (_time > Delay)
	    {
	        float t = (_time - Delay)/Lifetime;
	        transform.position = new Vector3(transform.position.x, Mathf.Lerp(_startY, _endY, t), transform.position.z);
	        Color c = renderer.material.color;
            renderer.material.color = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, 0f, t / 1.5f));
	    }
	}
}
