using UnityEngine;

public class BlindStatusEffect : IActorStatusEffect
{
    public float BlindLength;
    public float OriginalAttackChance = -1f;

    private float _t;

    public bool Expired { get; private set; }

    public BlindStatusEffect(float blindLength)
    {
        BlindLength = blindLength;
    }

    public void OnAdd(Actor actor)
    {
        OriginalAttackChance = actor.CwInfo.AttackChance;
        actor.CwInfo.AttackChance = OriginalAttackChance / 2f;
    }

    public void ApplyEffect(Actor actor)
    {
        _t += Time.deltaTime;

        if (_t > BlindLength)
            OnExpire(actor);
    }

    public void OnExpire(Actor actor)
    {
        actor.CwInfo.AttackChance = OriginalAttackChance;
        Debug.Log(actor.DisplayName + " is no longer blinded.");
        Expired = true;
    }
}