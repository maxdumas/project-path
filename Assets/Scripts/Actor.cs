using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    public int Health;
    public int BaseAttack;
    public int BaseDefense;
    public Item WeaponSlot;
    public Item ShieldSlot;
    public Item MiscSlot;

    /// <summary>
    /// Gets an offensive "roll" for the player by combining the attack
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetAttackValue()
    {
        int attack = BaseAttack;
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
        int defense = BaseDefense; // Innate defense value
        if (WeaponSlot != null) defense += WeaponSlot.DefenseModifier(); // Who knows, maybe it can have one!
        if (ShieldSlot != null) defense += ShieldSlot.DefenseModifier();
        if (MiscSlot != null) defense += MiscSlot.DefenseModifier();

        return defense;
    }
}
