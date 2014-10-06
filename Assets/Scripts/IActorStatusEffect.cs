using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void ExpiredEventHandler(IActorStatusEffect sender, EventArgs e);

public interface IActorStatusEffect
{
    string Name { get; }

    bool IsExpired { get; }

    event ExpiredEventHandler Expired;

    void OnAdd(Actor actor);

    void ApplyEffect(Actor actor);

    void OnExpire(Actor actor);
}
