using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ChildMonster : Monster
{
    public Monster Master;
    public bool Buff = false;

    // Update is called once per frame
    public void Update()
    {
        if (Buff)
        {
            GetBuffModifier();
            Buff = false;
        }
    }

    public void GetBuffModifier()
    {
        if (!string.IsNullOrEmpty(name) && BuffBehaviors.ContainsKey(name))
            BuffBehaviors[name](Master);
    }

    public bool MasterExists()
    {
        return Master != null;
    }

    protected readonly Dictionary<string, Action<Monster>> BuffBehaviors = new Dictionary<string, Action<Monster>>
    {
        {
            "Red Wing", master =>
            {
                master.Health -= 5;
                master.BaseAttack -= 1;
            }
        }
    };
}