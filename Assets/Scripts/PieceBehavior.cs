using UnityEngine;
using System.Collections;

public abstract class PieceBehavior : MonoBehaviour
{
    public Player Player;
    public TextMesh EventNotifier;
    public float WaitTime = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player")) // Check if the other piece is the player
        {
            StartCoroutine(OnInteractionBegin());
        }
    }

    protected virtual IEnumerator OnInteractionBegin()
    {
        Player.PathFollower.AllowContinue = false;
        yield return StartCoroutine(OnInteraction(WaitTime));
        OnInteractionEnd();
    }

    protected abstract IEnumerator OnInteraction(float waitTime);

    protected virtual void OnInteractionEnd()
    {
        Player.PathFollower.AllowContinue = true;
    }

    protected void DisplayMessage(string text, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 offset = new Vector3(xOffset, yOffset, zOffset);

        DisplayMessage(text, offset);
    }

    protected void DisplayMessage(string text, Vector3 offset)
    {
        TextMesh message = (TextMesh)Instantiate(EventNotifier, transform.position + offset, Quaternion.identity);
        message.color = Color.red;
        message.text = text;
    }
}
