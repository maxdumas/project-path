using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : PieceBehavior
{
    public Monster MonsterPrefab; //MonsterPrefab;
    public SpriteRenderer MonsterSpritePrefab;
    public SpriteRenderer PlayerSpritePrefab;
    public SpriteRenderer BackgroundPrefab;
    public CombatWindow combat;
    public TextAsset MonsterPattern;

    private StringBuilder _log;

    protected override string Description
    {
        get { return MonsterPrefab == null ? "" : "A lair with the following monster: " + MonsterPrefab.MonsterDescription; }
    }

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
        combat.MonsterSpritePrefab = this.MonsterSpritePrefab;
        combat.Player = Player;
        combat.PlayerSpritePrefab = this.PlayerSpritePrefab;
        combat.BackgroundPrefab = this.BackgroundPrefab;
        combat.EventNotifier = EventNotifier;
        combat.MonsterPattern = MonsterPattern;
        combat.Enable();

        /*
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
        */
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

    protected override void OnGUI()
    {
        GUI.skin.box.wordWrap = true;
        if (ShowInfoState == 2)
            GUI.Box(new Rect(ClickLocation.x - 50, ClickLocation.y - 120, 100, 100), MonsterPrefab.MonsterDescription);
    }
}