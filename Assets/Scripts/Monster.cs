using UnityEngine;
using System.Collections.Generic;


public class Monster
{
	public int Health;
	public Item WeaponSlot;
	public Item ShieldSlot;
	public Item MiscSlot;
	public int Defense;
	public readonly List<Item> AllItems = new List<Item>();

	/// <summary>
	/// Gets an offensive "roll" for the monster by combining the attack
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

	public int GetDefenseValue()
	{
		int defense = Defense; // Innate defense value
		if (WeaponSlot != null) defense += WeaponSlot.DefenseModifier(); // Who knows, maybe it can have one!
		if (ShieldSlot != null) defense += ShieldSlot.DefenseModifier();
		if (MiscSlot != null) defense += MiscSlot.DefenseModifier();
		
		return defense;
	}
}

