using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : PieceBehavior
{
    public Monster MonsterPrefab; //MonsterPrefab;
    public SpriteRenderer BackgroundPrefab;
    public SpriteRenderer FramePrefab;
    public CombatWindow combat;
    public TextAsset MonsterPattern;
    public GameObject cube;

    private StringBuilder _log;

    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("COMBAT LOG:\n");
        return base.OnInteractionBegin();
        
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        combat = (CombatWindow)Instantiate(combat, transform.position, Quaternion.identity);
        combat.MonsterPrefab = this.MonsterPrefab;
        combat.Player = Player;
        combat.BackgroundPrefab = this.BackgroundPrefab;
        combat.FramePrefab = this.FramePrefab;
        combat.EventNotifier = EventNotifier;
        combat.MonsterPattern = MonsterPattern;
        combat.cube = this.cube;
        combat.Enable();
    }

    protected override void OnInteractionEnd()
    {
        Debug.Log(_log.ToString());
        SendMessage("StartChangeLevel");
        Destroy(gameObject);
        base.OnInteractionEnd();
    }

    private void LogMessage(string message)
    {
        _log.AppendLine(message);
        DisplayMessage(message);
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && ShowInfoState == 0)
                ShowInfoState = 1;
            else
                ShowInfoState = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && ShowInfoState == 1)
            {
                ShowInfoState = 2;
                ClickLocation = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            }
            else
                ShowInfoState = 0;
        }
    }
}