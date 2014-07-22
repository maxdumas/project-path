using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : MonoBehaviour
{
	public TextMesh EventNotifier;
	public Player Player;
	public Monster MonsterPrefab;

    private void OnTriggerEnter2D(Collider2D other) // Called when another piece intersects this piece
    {
        if (other.tag.Equals("Player") && MonsterPrefab != null) // Check if the other piece is the player
        {
            // Instantiate a new text message.
            TextMesh message = (TextMesh) Instantiate(EventNotifier, transform.position, Quaternion.identity);
            StringBuilder text = new StringBuilder("COMBAT LOG:");

            //Spawn the monster
            Monster enemy = (Monster) Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
            text.AppendLine("Combat between Player (" + Player.Health + "HP) and a(n) " + enemy.name + " (" +
                            enemy.Health + "HP).");

            while (Player.Health > 0 && enemy.Health > 0)
            {
                int attackValue = Player.GetAttackValue();
                int defenseValue = enemy.GetDefenseValue();
                int dmg = attackValue - defenseValue;
                // Display attack
                text.AppendLine("You attack for " + attackValue + "!");
                text.AppendLine("Monster defends for " + defenseValue + "!");
                if (dmg > 0)
                {
                    enemy.Health -= dmg;
                    text.AppendLine("Hit! You deal " + dmg + " damage! Their health now " + enemy.Health);
                    if (enemy.Health <= 0)
                    {
                        text.AppendLine("Monster dies!");
                        break;
                    }
                }
                else
                {
                    text.AppendLine("They dodge your attack!");
                }

                // We switch sides now!
                attackValue = enemy.GetAttackValue();
                defenseValue = Player.GetDefenseValue();
                dmg = attackValue - defenseValue;

                text.AppendLine("Monster attacks for " + attackValue);
                text.AppendLine("You defend for " + defenseValue);
                if (dmg > 0)
                {
                    Player.Health -= dmg;
                    text.AppendLine("You are hit for " + dmg + " damage! Your health now " + Player.Health);
                    if (Player.Health <= 0)
                    {
                        text.AppendLine("You died! Game Over!");
                        break;
                    }
                }
                else
                {
                    text.AppendLine("You parry the blow!");
                }
            }

            Destroy(enemy.gameObject);

            Debug.Log(text.ToString());
            message.text = text.ToString();
        }
    }
}