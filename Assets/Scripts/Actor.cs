using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public IEnumerable<Item> AllSlots
    {
        get
        {
            if(WeaponSlot != null) yield return WeaponSlot;
            if(ShieldSlot != null) yield return ShieldSlot;
            if(MiscSlot != null) yield return MiscSlot;
        }
    }

	//Poison Attack Values
	public bool IsPoisonous;
	public float PoisonChance;
	public int PoisonDamageValue;
	public float PoisonTickSpeed;

	//Accuracy and Evasion Values
	public float Accuracy;
	public float Evasion;

	//Blind Values
	public bool IsBlinding;
	public float BlindChance;
	public float BlindAttackLength;

    public readonly IDictionary<string, IActorStatusEffect> StatusEffects = new Dictionary<string, IActorStatusEffect>();
    


    public virtual void Start()
    {
        if (MaxHealth < Health) MaxHealth = Health;

        if (String.IsNullOrEmpty(DisplayName)) DisplayName = name;
    }

    public virtual void Update()
    {
        var expiredEffects = new List<string>();
        foreach(var kvp in StatusEffects)
            if (!kvp.Value.Expired)
                kvp.Value.ApplyEffect(this);
            else expiredEffects.Add(kvp.Key);

        foreach (string s in expiredEffects)
            StatusEffects.Remove(s);
    }

    /// <summary>
    /// Gets an offensive "roll" for the player by combining the attack
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetAttackValue()
    {
        return BaseAttack + AllSlots.Sum(item => item.AttackModifier());
    }

    /// <summary>
    /// Gets a defensive "roll" for the player by combining the defense
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetDefenseValue()
    {
        return BaseDefense + AllSlots.Sum(item => item.DefenseModifier());
    }

    public void AddStatusEffect(string effectName, IActorStatusEffect effect)
    {
        if (StatusEffects.ContainsKey(effectName))
            StatusEffects[effectName] = effect; // Replace the existing effect (thus refreshing it)
        else StatusEffects.Add(effectName, effect);

        effect.OnAdd(this);
    }
}
