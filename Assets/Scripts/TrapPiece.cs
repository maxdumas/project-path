using UnityEngine;
using System.Collections;

public class TrapPiece : MonoBehaviour
{
    public TextMesh EventNofifier;
    public Player Player;
    public float ChanceToDamage = 0.5f;
    public int Damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DOOBUMS");
        if (other.tag.Equals("Player"))
        {
            //TextMesh message = Instantiate(EventNofifier, transform.position, Quaternion.identity) as TextMesh;
            float roll = Random.Range(0f, 1f);
            if (roll > ChanceToDamage)
            {
                Player.Health -= Damage;
                // Display Message regarding trap hit
                //message.text = "Trap hits successfully for " + Damage + " damage!";
                Debug.Log("Doobies");
            }
            else
            {
                //message.text = "Trap misses!";
            }
        }
    }
}
