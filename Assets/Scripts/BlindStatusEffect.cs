using UnityEngine;

public class BlindStatusEffect : IActorStatusEffect
{
    public float BlindLength;
    public float OriginalAccuracy = -1f;

    private float _t;

    public bool Expired { get; private set; }

    public BlindStatusEffect(float blindLength)
    {
        BlindLength = blindLength;
    }

    public void OnAdd(Actor actor)
    {
        OriginalAccuracy = actor.Accuracy;
        actor.Accuracy = OriginalAccuracy / 2f;
    }

    public void ApplyEffect(Actor actor)
    {
        _t += Time.deltaTime;

        if (_t > BlindLength)
            OnExpire(actor);
    }

    public void OnExpire(Actor actor)
    {
        actor.Accuracy = OriginalAccuracy;
        Debug.Log(actor.DisplayName + " is no longer blinded.");
        Expired = true;
    }
}