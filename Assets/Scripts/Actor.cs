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

    public SpriteRenderer PrefabCombatSprite;
    public SpriteRenderer CombatSprite;
    public Animator CombatAnimator;

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

    public readonly HashSet<IActorStatusEffect> StatusEffects = new HashSet<IActorStatusEffect>();
    
    public virtual void OnEnable()
    {
        CombatSprite = (SpriteRenderer) Instantiate(PrefabCombatSprite);
        CombatSprite.enabled = false;
        CombatAnimator = CombatSprite.GetComponent<Animator>();

        if (MaxHealth < Health) MaxHealth = Health;

        if (String.IsNullOrEmpty(DisplayName)) DisplayName = name;
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

    public int GetArmorValue()
    {
        if (!(MiscSlot == null))
        {
            return BaseDefense + MiscSlot.DefenseModifier();
        }
        else
        {
            return BaseDefense;
        }
    }

    public int GetShieldValue()
    {
        if (!(ShieldSlot == null))
        {
            return BaseDefense + ShieldSlot.DefenseModifier();
        } 
        else
        {
            return 0;
        }
    }

    public void AddStatusEffect(IActorStatusEffect effect)
    {
        if (!StatusEffects.Add(effect))
        {
            StatusEffects.Remove(effect);
            StatusEffects.Add(effect);
        }
        effect.OnAdd(this);
        effect.Expired += RemoveExpiredStatusEffect;
    }

    private void RemoveExpiredStatusEffect(IActorStatusEffect sender, EventArgs e)
    {
        StatusEffects.Remove(sender);
    }
}
