using UnityEngine;
using System.Collections;

public class FadeAndRise : MonoBehaviour
{
    public float Lifetime = 1f;
    public float Delay = 0f;
    public float EndYOffset;

    private float time = 0f;
    private float StartY, EndY;
    private TextMesh text;

	// Use this for initialization
	void Start ()
	{
	    StartY = transform.position.y;
	    EndY = transform.position.y + EndYOffset;
	    text = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    time += Time.deltaTime;

	    if (time > Delay)
	    {
	        float a = (time - Delay)/Lifetime;
	        transform.position.Set(transform.position.x, Mathf.Lerp(StartY, EndY, a), transform.position.z);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0f, a));
            if(time - Delay > Lifetime)
                Destroy(gameObject);
	    }
	}
}
