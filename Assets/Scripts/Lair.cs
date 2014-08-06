using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : PieceBehavior
{
    public Monster enemy; //MonsterPrefab;
	public SpriteRenderer PlayerSpritePrefab;
	public SpriteRenderer BackgroundPrefab;
    public CombatWindow combat;

    private StringBuilder _log;


    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("COMBAT LOG:\n");
        return base.OnInteractionBegin();
        
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

		combat = (CombatWindow)Instantiate(combat,transform.position,Quaternion.identity);
		combat.MonsterPrefab = this.enemy;
		combat.player = Player;
		combat.PlayerSpritePrefab = this.PlayerSpritePrefab;
		combat.BackgroundPrefab = this.BackgroundPrefab;
        combat.Enable();
    }

    protected override void OnInteractionEnd()
    {
        Debug.Log(_log.ToString());
        base.OnInteractionEnd();
    }

    private void LogMessage(string message)
    {
        _log.AppendLine(message);
        DisplayMessage(message);
    }

    private int _showInfoState = 0;
    private Vector2 _clickLocation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && _showInfoState == 0)
                _showInfoState = 1;
            else _showInfoState = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (transform.parent.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && _showInfoState == 1)
            {
                _showInfoState = 2;
                _clickLocation = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            }
            else _showInfoState = 0;
        }
    }

    void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        if(_showInfoState == 2)
            GUI.Box(new Rect(_clickLocation.x - 50, _clickLocation.y - 120, 100, 100), enemy.MonsterDescription);
    }

}