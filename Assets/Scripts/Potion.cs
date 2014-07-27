using System.Collections;
using System.Text;
using UnityEngine;

public class Potion : PieceBehavior
{
    protected override string Description
    {
        get { return "A potion! Heals 3 health."; }
    }

    protected override IEnumerator OnInteraction(float waitTime)
    {
        LogMessage("Player healed for 3!");

        if (Player.Health + 3 > Player.MaxHealth)
            Player.Health = Player.MaxHealth;
        else Player.Health += 3;
        
        yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
    }

    private void LogMessage(string message)
    {
        Debug.Log(message);
        DisplayMessage(message);
    }
}
