using UnityEngine;
using System.Collections;

public abstract class PieceBehavior : MonoBehaviour
{
    public Player Player;
    public TextMesh EventNotifier;
    public float WaitTime = 0.5f;

    protected int ShowInfoState = 0;
    protected Vector2 ClickLocation;

    protected abstract string Description { get; }

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

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && ShowInfoState == 0)
                ShowInfoState = 1;
            else ShowInfoState = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && ShowInfoState == 1)
            {
                ShowInfoState = 2;
                ClickLocation = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            }
            else ShowInfoState = 0;
        }
    }

    protected virtual void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        if (ShowInfoState == 2)
            GUI.Box(new Rect(ClickLocation.x - 75, ClickLocation.y - 170, 150, 150), Description);
    }
}
