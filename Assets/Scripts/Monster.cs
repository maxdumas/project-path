using UnityEngine;
using System;
using System.Collections.Generic;


public class Monster : Actor
{
	public Monster Master;
	public bool buff = false;
    public string MonsterName;

	void Update() {
		if (buff) {
            getBuffModifier();
            buff = false;
		}
	}

    public void getBuffModifier()
    {
        if (!string.IsNullOrEmpty(MonsterName) && BuffBehaviors.ContainsKey(MonsterName))
            BuffBehaviors[MonsterName](this);
    }

    public bool MasterExists()
    {
        return !(Master == null);
    }

    public readonly Dictionary<string, Action<Monster>> BuffBehaviors = new Dictionary<string, Action<Monster>>
	{
		{"RedWing", (Monster m) =>
		{
			m.Master.Health -= 5;
			m.Master.BaseAttack -=1;
		}}
	};

}

