using System.Collections;
using System.Text;
using UnityEngine;

public class Potion : PieceBehavior
{
    private StringBuilder _log;

    protected override IEnumerator OnInteractionBegin()
    {
        _log = new StringBuilder("");
        return base.OnInteractionBegin();
    }
    protected override IEnumerator OnInteraction(float waitTime)
    {
        LogMessage("Player healed for 3!");

        if (Player.Health + 3 > Player.MaxHealth)
        {
            Player.Health = Player.MaxHealth;
        }
        else 
        {
            Player.Health += 3;
        }
        
        yield return new WaitForSeconds(waitTime);
		Destroy(this.gameObject);
    }

    private void LogMessage(string message)
    {
        _log.AppendLine(message);
        DisplayMessage(message);
    }
}
