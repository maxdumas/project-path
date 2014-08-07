using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static readonly Func<int> DefaultBehavior = () => 0;

    public string DisplayName;
    public ItemType Type;
    public string AttackBehaviorName;
    public string DefenseBehaviorName;

    public Func<int> AttackModifier
    {
        get
        {
            if (_attackModifier != null) return _attackModifier;
            if (!string.IsNullOrEmpty(AttackBehaviorName) && AttackBehaviors.ContainsKey(AttackBehaviorName))
                return _attackModifier = AttackBehaviors[AttackBehaviorName];

            return _attackModifier = DefaultBehavior;
        }
    }

    public Func<int> DefenseModifier
    {
        get
        {
            if (_defenseModifier != null) return _defenseModifier;
            if (!string.IsNullOrEmpty(DefenseBehaviorName) && DefenseBehaviors.ContainsKey(DefenseBehaviorName))
                return _defenseModifier = DefenseBehaviors[DefenseBehaviorName];
            return _defenseModifier = DefaultBehavior;
        }
    }

    public string ItemDescription
    {
        get
        {
            if (!string.IsNullOrEmpty(_itemDescription))
                return _itemDescription;
            if (ItemDescriptors.ContainsKey(name))
                _itemDescription = ItemDescriptors[name];
            else _itemDescription = "";
            return _itemDescription;
        }
    }

    private Func<int> _attackModifier;
    private Func<int> _defenseModifier;
    private string _itemDescription = null;

	// Use this for initialization
	void Start ()
	{
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)Type;
            return hashCode;
        }
    }

    public override bool Equals(object o)
    {
        if (ReferenceEquals(null, o)) return false;
        if (ReferenceEquals(this, o)) return true;
        if (o.GetType() != this.GetType()) return false;
        Item other = (Item) o;
        return base.Equals(other) && string.Equals(DisplayName, other.DisplayName) && Type == other.Type;
    }

    // These fields are down here so they don't get in our way when we edit the above code
    // They will probably get huge.
    // If a negative number is returned it is assumed that all damage is negated.
    public static readonly Dictionary<string, Func<int>> AttackBehaviors = new Dictionary<string, Func<int>>
    {
        {"Fists", () => Dice.Roll(4)},
		{"Fists+1",() => 
		{
			int d = Dice.Roll(4);
			return d+1;
		}},
        {"Dagger", () =>
        {
            int d = Dice.Roll(4);
            return d == 4 ? 5 : d; // Roll increases to a 5 on a critical hit.
        }},
		{"Short Sword", () => Dice.Roll(6)},
		{"Short Sword +1", () => 
		{
			int d = Dice.Roll(6);
			return d+1;
		}},
		{"Spear", () => Dice.Roll(8)},
		{"Spear +1", () =>
		{
			int d = Dice.Roll(8);
			return d+1;
		}}
    };

    public static readonly Dictionary<string, Func<int>> DefenseBehaviors = new Dictionary<string, Func<int>>
    {
        {"Buckler", () => Dice.Roll(4)},
		{"Leather Armor", () => 1},
		{"Heater", () => Dice.Roll(6)},
		{"Bronze Armor", () => 2},
		{"Iron Armor", () => 3},
		{"Steel Armor", () => 4},
		{"Greatshield", () => Dice.Roll(8)},
    };

	public static readonly Dictionary<string,string> ItemDescriptors = new Dictionary<string, string>
	{
		{"Dagger", "A small but sharp dagger. Fits neatly between the ribs. Roll: d4 Effect: All 4 become 5."},
		{"Short Sword", "A short but sturdy sword. Perfect for an adventurer. Roll: d6 Effect: None."},
		{"Short Sword +1", "A short sword that's been sharpened well. Roll: d6+1 Effect: None."},
		{"Spear", "A long soldier's spear. The shaft shows wear but holds true. Roll: d8 Effect: None."},
		{"Spear +1", "A soldier's spear fitted with a diamond-shaped head. Strong and sturdy. Roll: d8+1 Effect: None."},
		{"Buckler", "A round, small parrying shield. Not designed to take full frontal blows. Roll: d4 Effect: None."},
		{"Heater", "A plain knight's shield. The crest has worn off over time. Roll: d6 Effect: None."},
		{"Greatshield", "A heavy square greatshield. A solid core can deflect the heaviest of blows. Roll: d8 Effect: None."},
		{"Leather Armor", "A worn and comfortable leather kit. Light and mobile, but not too sturdy. Effect: +1 Defense."},
		{"Bronze Armor", "Shoddily crafted bronze armor. Good for a squire. Effect: +2 Defense."},
		{"Iron Armor", "A complete set of iron armor. Good for a travelling adventurer. Effect: +3 Defense."},
		{"Steel Armor", "Shining steel armor. Even the scratches glint in the sunlight. Effect: +4 Defense."}
	};
	
}