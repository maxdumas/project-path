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
            StringBuilder text = new StringBuilder();

            //Spawn the monster
            Monster enemy = (Monster) Instantiate(MonsterPrefab, transform.position, Quaternion.identity);
            //while (Player.Health > 0 && Monster.Health > 0)
            //{
                int attackValue = Player.GetAttackValue();
                int defenseValue = enemy.GetDefenseValue();
                int dmg = attackValue - defenseValue;

                // Display attack
                Debug.Log("You attack for " + attackValue + "!");
                Debug.Log("Monster defends for " + defenseValue + "!");
                if (dmg > 0)
                {
                    enemy.Health -= dmg;
                    Debug.Log("Hit! You deal " + dmg + " damage! Their health now " + enemy.Health);
                    if (enemy.Health <= 0)
                    {
                        Debug.Log("Monster dies!");
                        //break;
                    }
                }
                else
                {
                    Debug.Log("They dodge your attack!");
                }

                // We switch sides now!
                attackValue = enemy.GetAttackValue();
                defenseValue = Player.GetDefenseValue();
                dmg = attackValue - defenseValue;

                Debug.Log("Monster attacks for " + attackValue);
                Debug.Log("You defend for " + defenseValue);
                if (dmg > 0)
                {
                    Player.Health -= dmg;
                    Debug.Log("You are hit for " + dmg + " damage! Your health now " + Player.Health);
                    if (Player.Health <= 0)
                    {
                        Debug.Log("You died! Game Over!");
                        //break;
                    }
                }
                else
                {
                    Debug.Log("You parry the blow!");
                }

                //Debug.Break();
            //}

            Debug.Log(text.ToString());
            message.text = text.ToString();
        }
    }
}