using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    public TextMesh EventNotifier;
    public Player Player;
    public Item ContainedItem;

    private void OnTriggerEnter2D(Collider2D other) // Called when another piece intersects this piece
    {
        if (other.tag.Equals("Player")) // Check if the other piece is the player
        {
            // Instantiate a new text message.
            TextMesh message = (TextMesh) Instantiate(EventNotifier, transform.position, Quaternion.identity);
            string text = "";

            if (ContainedItem == null)
                text = "There's nothing here...";
            else
            {
                if (Player.EquipNewItem(ContainedItem))
                    text = ContainedItem.DisplayName + " acquired! It has been equipped.";
                else text = "You already have one of these.";
                ContainedItem = null;
            }
			message.color = Color.red;
            message.text = text;
            Debug.Log(text);
        }
    }
}
