using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Collections;

public class Lair : MonoBehaviour
{

	public Player Player;
	public Monster MonsterPrefab;
	public TextMesh EventNotifier;
	public StringBuilder text;


    private void OnTriggerEnter2D(Collider2D other) // Called when another piece intersects this piece
    {
        if (other.tag.Equals("Player") && MonsterPrefab != null) // Check if the other piece is the player
        {
            // Instantiate a new text message.
			text = new StringBuilder("COMBAT LOG:\n");
			StartCoroutine(Combat(0.5f));
		}
           
    }

	IEnumerator Combat(float waitTime) {
	
		string t;

		//Spawn the monster
		Monster enemy = (Monster) Instantiate(MonsterPrefab, transform.position, Quaternion.identity);

		//Print start of combat
		t = "Combat between Player (" + Player.Health + "HP) and a(n) " + enemy.name + " (" +
			enemy.Health + "HP).";
		DisplayMsg(t);
		yield return new WaitForSeconds(waitTime);

		while (Player.Health > 0 && enemy.Health > 0)
		{
			int attackValue = Player.GetAttackValue();
			int defenseValue = enemy.GetDefenseValue();
			int dmg = attackValue - defenseValue;

			// Display player attack
			t = "You attack for " + attackValue + "!";
			DisplayMsg(t);
			yield return new WaitForSeconds(waitTime);

			//Display monster defend
			t = "Monster defends for " + defenseValue + "!";
			DisplayMsg(t);
			yield return new WaitForSeconds(waitTime);
			
			if (dmg > 0)
			{
				enemy.Health -= dmg;

				//Display successful player hit
				t = "Hit! You deal " + dmg + " damage! Their health now " + enemy.Health;
				DisplayMsg(t);
				yield return new WaitForSeconds(waitTime);

				if (enemy.Health <= 0)
				{
					//Display monster death
					t = "Monster dies!";
					DisplayMsg(t);
					yield return new WaitForSeconds(waitTime);

					//Combat over; end while loop
					break;
				}
			}
			else
			{
				//Display failed player hit
				t = "They dodge your attack!";
				DisplayMsg(t);
				yield return new WaitForSeconds(waitTime);
			}
			
			
			// We switch sides now!
			attackValue = enemy.GetAttackValue();
			defenseValue = Player.GetDefenseValue();
			dmg = attackValue - defenseValue;

			//Display monster attack
			t = "Monster attacks for " + attackValue;
			DisplayMsg(t);
			yield return new WaitForSeconds(waitTime);


			//Display player defend
			t = "You defend for " + defenseValue;
			DisplayMsg(t);
			yield return new WaitForSeconds(waitTime);

			if (dmg > 0)
			{
				Player.Health -= dmg;

				//Display successful monster hit
				t = "You are hit for " + dmg + " damage! Your health now " + Player.Health;
				DisplayMsg(t);
				yield return new WaitForSeconds(waitTime);

				if (Player.Health <= 0)
				{
					//Display player death
					t = "You died! Game Over!";
					DisplayMsg(t);
					yield return new WaitForSeconds(waitTime);

					//Combat over; end while loop
					break;
				}
			}
			else
			{
				//Display failed monster hit
				t = "You parry the blow!";
				DisplayMsg(t);
				yield return new WaitForSeconds(waitTime);
			}
		}
		
		Destroy(enemy.gameObject);	
		Debug.Log(text.ToString());
	}

	public void DisplayMsg(string t) {

		float x = transform.position.x;
		float y = transform.position.y;
		Vector3 v = new Vector3(x,y+1.0f,transform.position.z);

		text.AppendLine(t);
		TextMesh message = (TextMesh) Instantiate(EventNotifier, v, Quaternion.identity);
		message.color = Color.red;
		message.text = t;
	}

}