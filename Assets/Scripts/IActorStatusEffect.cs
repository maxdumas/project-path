using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IActorStatusEffect
{
    bool Expired { get; }

    void OnAdd(Actor actor);

    void ApplyEffect(Actor actor);

    void OnExpire(Actor actor);
}
