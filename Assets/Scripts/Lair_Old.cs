using System.Text;
using UnityEngine;
using System.Collections;

public class Lair_Old //: PieceBehavior
{/*
    public Monster enemy; //MonsterPrefab;
    //public CombatWindow combat;
    
    //public SpriteRenderer x;

    private StringBuilder _log;

    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("COMBAT LOG:\n");
        //x.transform.position = Player.transform.position;
        //x.sortingLayerName = "front";
        //combat.enable();
        return base.OnInteractionBegin();
        
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //combat.enable();
        //Spawn the monster
        //Monster enemy = (Monster) Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
        
        //Print start of combat
        LogMessage("Combat between Player (" + Player.Health + "HP) and a(n) " + enemy.name + " (" + enemy.Health +
                   "HP).");
        yield return new WaitForSeconds(waitTime);

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

        if (enemy.MasterExists())
        {
            enemy.Buff = true;
            LogMessage("Debuff activated for " + enemy.Master.MonsterName + "!");
            yield return new WaitForSeconds(waitTime);
        }
        Destroy(gameObject);
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
    }*/
}