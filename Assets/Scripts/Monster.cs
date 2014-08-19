using System.Collections.Generic;


public class Monster : Actor
{
    public int NumAttacks;

    public string MonsterDescription
    {
        get
        {
            if (!string.IsNullOrEmpty(_monsterDescription))
                return _monsterDescription;
            if (MonsterDescriptors.ContainsKey(name))
                _monsterDescription = MonsterDescriptors [name];
            else
                _monsterDescription = "";
            return _monsterDescription;
        }
    }

    private string _monsterDescription = null;

    public readonly Dictionary<string, string> MonsterDescriptors = new Dictionary<string, string> 
    {
        {"Slime","A cuddly slime! Health: 2 Attack: d4 + 1 Defense: 1"},
        {"Slime +1","A less cuddly slime! Health: 2 Attack: d4 + 2 Defense: 1"},
        {"Drop Duke", "The duke of slimes! Health: 5 Attack d4 + 2 Defense: 2"},
        {"Snake", "It's a snaaaake~ Health: 3 Attack: d4 + 1 Defense:3"},
        {"Giant Rat", "From a famous swamp. Health: 3 Attack: d6 + 1 Defense: 3"},
        {"Tentacle", "A girl's best friend. Health: 4 Attack: d6 + 1 Defense: 2"},
        {"Giant Squid", "Straight out of a nightmare. Health: 7 Attack: d6 + 2 Defense: 3"},
        {"Skeleton", "Hugs aren't his thing. Health: 5 Attack: d6 + 1 Defense: 3"},
        {"Undead Dog", "Skeleton's best friend. Health: 5 Attack: d4 + 3 Defense: 3"},
        {"Skeleton King", "The skeleton king. Health: 10, Attack d6 + 2 Defense: 4"},
        {"Red Wing", "A wing of the red dragon. Break it! Health: 7 Attack: d6 +3 Defense: 4"},
        {"Red Dragon", "He ate your mother. Break his wings to cripple him! Health: 25 Attack: d8 + 2 Defense: 5"}
    };
}

