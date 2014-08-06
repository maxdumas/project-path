using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : PieceBehavior
{
    public Monster enemy; //MonsterPrefab;
<<<<<<< HEAD
	public SpriteRenderer PlayerSpritePrefab;
	public SpriteRenderer BackgroundPrefab;
    public CombatWindow combat;

    private StringBuilder _log;

=======

    private StringBuilder _log;

    protected override string Description
    {
        get { return enemy == null ? "" : "A lair with the following monster: " + enemy.MonsterDescription; }
    }
>>>>>>> 345946165b188acbc47b2693212afcac00585451

    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("COMBAT LOG:\n");
        return base.OnInteractionBegin();
        
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

<<<<<<< HEAD
		combat = (CombatWindow)Instantiate(combat,transform.position,Quaternion.identity);
		combat.MonsterPrefab = this.enemy;
		combat.player = Player;
		combat.PlayerSpritePrefab = this.PlayerSpritePrefab;
		combat.BackgroundPrefab = this.BackgroundPrefab;
        combat.Enable();
=======
        while (Player.Health > 0 && enemy.Health > 0)
        {
            int attackValue = Player.GetAttackValue();
            int defenseValue = enemy.GetDefenseValue();
            int dmg = attackValue - defenseValue;
            
            // Display player attack
            LogMessage("You attack for " + attackValue + "!");
            yield return new WaitForSeconds(waitTime);

            //Display monster defend
            LogMessage("Monster defends for " + defenseValue + "!");
            yield return new WaitForSeconds(waitTime);

            if (dmg > 0)
            {
                enemy.Health -= dmg;

                //Display successful player hit
                LogMessage("Hit! You deal " + dmg + " damage! Their health now " + enemy.Health);
                yield return new WaitForSeconds(waitTime);

                if (enemy.Health <= 0)
                {
                    //Display monster death
                    LogMessage("Monster dies!");
                    yield return new WaitForSeconds(waitTime);

                    //Combat over; end while loop
                    break;
                }
            }
            else
            {
                //Display failed player hit
                LogMessage("They dodge your attack!");
                yield return new WaitForSeconds(waitTime);
            }

            // We switch sides now!
            attackValue = enemy.GetAttackValue();
            defenseValue = Player.GetDefenseValue();
            dmg = attackValue - defenseValue;

            //Display monster attack
            LogMessage("Monster attacks for " + attackValue);
            yield return new WaitForSeconds(waitTime);

            //Display player defend
            LogMessage("You defend for " + defenseValue);
            yield return new WaitForSeconds(waitTime);

            if (dmg > 0)
            {
                Player.Health -= dmg;

                //Display successful monster hit
                LogMessage("You are hit for " + dmg + " damage! Your health now " + Player.Health);
                yield return new WaitForSeconds(waitTime);

                if (Player.Health <= 0)
                {
                    //Display player death
                    LogMessage("You died! Game Over!");
                    yield return new WaitForSeconds(waitTime);

                    //Combat over; end while loop
                    break;
                }
            }
            else
            {
                //Display failed monster hit
                LogMessage("You parry the blow!");
                yield return new WaitForSeconds(waitTime);
            }
        }


        ChildMonster e = enemy as ChildMonster;
        if (e != null && e.MasterExists())
        {
            e.Buff = true;
            LogMessage("Debuff activated for " + e.Master.DisplayName + "!");
            yield return new WaitForSeconds(waitTime);
        }
        Destroy(enemy.gameObject);
        Destroy(gameObject);
>>>>>>> 345946165b188acbc47b2693212afcac00585451
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
<<<<<<< HEAD

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

=======
>>>>>>> 345946165b188acbc47b2693212afcac00585451
}