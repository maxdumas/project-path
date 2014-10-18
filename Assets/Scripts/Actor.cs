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

    public SpriteRenderer PrefabCombatSprite;
    public SpriteRenderer CombatSprite;
    public Animator CombatAnimator;

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
        return BaseAttack;
    }

    /// <summary>
    /// Gets a defensive "roll" for the player by combining the defense
    /// modifiers of all equipped items.
    /// </summary>
    /// <returns></returns>
    public int GetDefenseValue()
    {
        return BaseDefense;
    }

    public int GetArmorValue()
    {
        return BaseDefense;
    }

    public int GetShieldValue()
    {
        return 0;
    }
}
