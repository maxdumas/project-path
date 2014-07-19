using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will hold all data for the player, i.e. their health, their items, etc.
/// </summary>
public class Player : MonoBehaviour
{
    public int Level;
    public int Health;
    public readonly Dictionary<string, GameObject> Items = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
