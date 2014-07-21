using UnityEngine;
using System.Collections;

public class Lair : MonoBehaviour
{
	public TextMesh EventNotifier;
	public Player Player;
	public Monster Monster;
	
	private void OnTriggerEnter2D(Collider2D other) // Called when another piece intersects this piece
	{
		if (other.tag.Equals("Player")) // Check if the other piece is the player
		{
			// Instantiate a new text message.
			TextMesh message = (TextMesh) Instantiate(EventNotifier, transform.position, Quaternion.identity);
			string text = "";
			int playerAtt = 0;
			int playerDef = 0;
			int monsterAtt = 0;
			int dmg = 0;

			while (Player.Health > 0 && Monster.Health > 0) {
				playerAtt = Player.GetAttackValue();

				///Display attack
				///text = "You attack for " + playerAtt + "!";
				/// 
				dmg = playerAtt - Monster.Defense;
				if (dmg > 0) {
					///text = "Hit! You deal " + dmg + " damage!";
					Monster.Health -= dmg;
					if (Monster.Health <= 0) {
						///text = "Monster dies!"
						break;
					}
				} else {
					///text = "Fail! No damage dealt!";
				}

				monsterAtt = Monster.GetAttackValue();
				playerDef = Player.GetDefenseValue();

				///text = "Monster attacks for " + monsterAtt;
				///text = "You defend for " + playerDef;

				dmg = monsterAtt - playerDef;

				if (dmg > 0) {
					///text = "You are hit for " + dmg + " damage!"
					Player.Health -= dmg;
					if (Player.Health <= 0) {
						///text = "You died! Game Over!"
						break;
					}				
				}
			}
		}
	}
}