using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will hold all data for the player, i.e. their health, their items, etc.
/// </summary>
public class Player : MonoBehaviour
{
    public int Health;
    public Item WeaponSlot;
    public Item ShieldSlot;
    public Item MiscSlot;
    public readonly List<Item> AllItems = new List<Item>(); 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Gets an offensive "roll" for the player by combining the attack
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetAttackValue()
    {
        int attack = 0;
        if (WeaponSlot != null) attack += WeaponSlot.AttackModifier();
        if (ShieldSlot != null) attack += ShieldSlot.AttackModifier();
        if (MiscSlot != null) attack += MiscSlot.AttackModifier();
        return attack;
    }

    /// <summary>
    /// Gets a defensive "roll" for the player by combining the defense
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetDefenseValue()
    {
        int defense = 1; // Innate defense value
        if (WeaponSlot != null) defense += WeaponSlot.DefenseModifier(); // Who knows, maybe it can have one!
        if (ShieldSlot != null) defense += ShieldSlot.DefenseModifier();
        if (MiscSlot != null) defense += MiscSlot.DefenseModifier();

        return defense;
    }

    /// <summary>
    /// Equip an item as well as add it to the inventory if it does not already exist there.
    /// </summary>
    /// <param name="i">The new item to add</param>
    /// <returns>true if the item is not present already in the inventory, false if it is.</returns>
    public bool EquipNewItem(Item i)
    {
        switch (i.Type)
        {
            case ItemType.Weapon:
                WeaponSlot = i;
                break;
            case ItemType.Shield:
                ShieldSlot = i;
                break;
            case ItemType.Misc:
                MiscSlot = i;
                break;
        }

        if (AllItems.Contains(i)) return false;
        
        AllItems.Add(i);
        return true;
    }
}
