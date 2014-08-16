using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    public string DisplayName;
    public int MaxHealth;
    public int Health;
    public int BaseAttack;
    public int BaseDefense;
    public Item WeaponSlot;
    public Item ShieldSlot;
    public Item MiscSlot;

	//While Poisoned Values
	public bool IsPoisoned;
	public int TakingPoisonFadeValue;
	public float TakingPoisonTickSpeed;
	
	//Poison Attack Values
	public bool IsPoisonous;
	public float PoisonChance;
	public int PoisonDamageValue;
	public float PoisonTickSpeed;

	//Accuracy and Evasion Values
	public float Accuracy;
	public float Evasion;

	//While Blinded Values
	public bool IsBlinded;

	//Blind Values
	public bool IsBlinding;
	public float BlindChance;
	public float BlindAttackLength;




    public virtual void Start()
    {
        if (MaxHealth < Health) MaxHealth = Health;

        if (string.IsNullOrEmpty(DisplayName)) DisplayName = name;
    }

    /// <summary>
    /// Gets an offensive "roll" for the player by combining the attack
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetAttackValue()
    {
        int attack = BaseAttack;
        //if (WeaponSlot != null) attack += WeaponSlot.AttackModifier();
        //if (ShieldSlot != null) attack += ShieldSlot.AttackModifier();
        //if (MiscSlot != null) attack += MiscSlot.AttackModifier();
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
