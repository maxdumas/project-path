using UnityEngine;
using System;
using System.Collections.Generic;


public class Monster : Actor
{
	public Monster Master;
	public bool buff = false;
    public string MonsterName;
    public string DescriptorName;
    private string _monsterDescription = null;

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

    public string MonsterDescription
    {
        get
        {
            if (!string.IsNullOrEmpty(_monsterDescription))
                return _monsterDescription;
            if (MonsterDescriptors.ContainsKey(DescriptorName))
                _monsterDescription = MonsterDescriptors[DescriptorName];
            else _monsterDescription = "";
            return _monsterDescription;
        }
    }


    public readonly Dictionary<string, Action<Monster>> BuffBehaviors = new Dictionary<string, Action<Monster>>
	{
		{"RedWing", (Monster m) =>
		{
			m.Master.Health -= 5;
			m.Master.BaseAttack -=1;
		}}
	};

    public readonly Dictionary<string, string> MonsterDescriptors = new Dictionary<string, string> 
    {
        {"Slime","A cuddly slime! Health: 2 Attack: d4 + 1 Defense: 1"},
        {"Slime1","A less cuddly slime! Health: 2 Attack: d4 + 2 Defense: 1"},
        {"DropDuke", "The duke of slimes! Health: 5 Attack d4 + 2 Defense: 2"},
        {"Snake", "It's a snaaaake~ Health: 3 Attack: d4 + 1 Defense:3"},
        {"GiantRat", "From a famous swamp. Health: 3 Attack: d6 + 1 Defense: 3"},
        {"Tentacle", "A girl's best friend. Health: 4 Attack: d6 + 1 Defense: 2"},
        {"GiantSquid", "Straight out of a nightmare. Health: 7 Attack: d6 + 2 Defense: 3"},
        {"Skeleton", "Hugs aren't his thing. Health: 5 Attack: d6 + 1 Defense: 3"},
        {"UndeadDog", "Skeleton's best friend. Health: 5 Attack: d4 + 3 Defense: 3"},
        {"SkeletonKing", "The skeleton king. Health: 10, Attack d6 + 2 Defense: 4"},
        {"RedWing", "A wing of the red dragon. Break it! Health: 7 Attack: d6 +3 Defense: 4"},
        {"RedDragon", "He ate your mother. Break his wings to cripple him! Health: 25 Attack: d8 + 2 Defense: 5"}
    };
}

