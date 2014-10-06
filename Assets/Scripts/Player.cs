using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will hold all data for the player, i.e. their health, their items, etc.
/// </summary>
public class Player : Actor
{
    public PathFollower PathFollower;
    public List<Item> AllItems = new List<Item>();

    public override void OnEnable()
    {
        if (PathFollower == null) PathFollower = GetComponent<PathFollower>();

        base.OnEnable();
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
