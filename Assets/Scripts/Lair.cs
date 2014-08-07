using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : PieceBehavior
{
    public Monster enemy; //MonsterPrefab;

    private StringBuilder _log;

    protected override string Description
    {
        get { return enemy == null ? "" : "A lair with the following monster: " + enemy.MonsterDescription; }
    }

    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("COMBAT LOG:\n");
        return base.OnInteractionBegin();
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
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

      
        ChildMonster e = enemy as ChildMonster;
        if (e != null && e.MasterExists())
        {
            e.Buff = true;
            LogMessage("Debuff activated for " + e.Master.DisplayName + "!");
            yield return new WaitForSeconds(waitTime);
        }
        Destroy(enemy.gameObject);
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
}