using System.Collections;
using UnityEngine;

public class Trap : PieceBehavior
{
	public float ChanceToDamage;
	public float ChanceToBreak;
    public int Damage;

    protected override string Description
    {
        get { return "A trap! Muy peligroso!"; }
    }

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

		if (ChanceToBreak > 0 && Player.MiscSlot != null)
		{
			float brk = Random.Range(0f,1f);
			if (brk < ChanceToBreak)
			{

				text = "Trap breaks your " + Player.MiscSlot.DisplayName;
				Player.MiscSlot = null;
			}
			else text = "Trap misses break!";

			DisplayMessage(text);
			Debug.Log(text);
		}
        
        yield return new WaitForSeconds(waitTime);
		Destroy(this.gameObject);
    }
}
