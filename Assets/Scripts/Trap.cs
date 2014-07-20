using UnityEngine;

public class Trap : MonoBehaviour
{
    public TextMesh EventNotifier;
    public Player Player;
    public float ChanceToDamage = 0.5f;
    public int Damage;

    void OnTriggerEnter2D(Collider2D other) // Called when another piece intersects this piece
    {
        if (other.tag.Equals("Player")) // Check if the other piece is the player
        {
            // Instantiate a new text message.
            TextMesh message = (TextMesh)Instantiate(EventNotifier, transform.position, Quaternion.identity);
            string text = "";
            float roll = Random.Range(0f, 1f);
            if (roll > ChanceToDamage)
            {
                Player.Health -= Damage;
                // Display Message regarding trap hit
                text = "Trap hits successfully for " + Damage + " damage!";
            }
            else text = "Trap misses!";

            message.text = text;
            Debug.Log(text);
        }
    }
}
