using UnityEngine;
using System.Collections;

public class Chest : PieceBehavior
{
    public Item ContainedItem;

    protected override IEnumerator OnInteraction(float waitTime)
    {
        string text;

        if (ContainedItem == null)
            text = "There's nothing here...";
        else
        {
            if (Player.EquipNewItem(ContainedItem))
                text = ContainedItem.DisplayName + " acquired! It has been equipped.";
            else text = "You already have one of these.";
            ContainedItem = null;
        }

        DisplayMessage(text);
        Debug.Log(text);

        yield return new WaitForSeconds(waitTime);
    }
}
