using System.Collections;
using UnityEngine;

public class Trap : PieceBehavior
{
	public float ChanceToDamage;
    public int Damage;

    protected override IEnumerator OnInteraction(float waitTime)
    {
        string text;
        float roll = Random.Range(0f, 1f);
        if (roll < ChanceToDamage)
        {
            Player.Health -= Damage;
            // Display Message regarding trap hit
            text = "Trap hits successfully for " + Damage + " damage!";
        }
        else text = "Trap misses damage!";

		DisplayMessage(text);
		Debug.Log(text);
		yield return new WaitForSeconds(waitTime);

		Destroy(this.gameObject);
    }
}
