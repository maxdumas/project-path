using System;
using UnityEngine;

public class PoisonStatusEffect : IActorStatusEffect
{
    public int Damage; // The amount of damage that will be dealt on the next tick
    public float DamagePeriod; // The amount of time in between damage

    private float _t;

    public string Name { get { return "Poison"; } }
    public bool IsExpired { get; private set; }

    public event ExpiredEventHandler Expired;

    public PoisonStatusEffect(int initialDamage, float damagePeriod)
    {
        Damage = initialDamage;
        DamagePeriod = damagePeriod;
    }

    public void OnAdd(Actor actor)
    {
        actor.CombatSprite.color = new Color(230f / 255f, 0, 250f / 255f, 1);
    }

    public void ApplyEffect(Actor actor)
    {
        _t += Time.deltaTime;
        if (_t < DamagePeriod) return;

        // It is time to deal damage again, reset timer and deal damage
        _t = 0f;

        Debug.Log(actor.DisplayName + " takes " + Damage + " poison damage!");
        actor.Health -= Damage;

        Damage /= 2; // Decay damage

        // End poisoning if it is has petered out
        if (Damage <= 0)
            OnExpire(actor);
    }

    public void OnExpire(Actor actor)
    {
        actor.CombatSprite.color = Color.white;
        Debug.Log(actor.DisplayName + " is no longer poisoned.");
        IsExpired = true;
        Expired(this, EventArgs.Empty);
    }
}