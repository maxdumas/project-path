using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private static readonly Func<int> DefaultBehavior = () => 0;

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
            if (_defenseModifier != null) return _attackModifier;
            if (!string.IsNullOrEmpty(DefenseBehaviorName) && DefenseBehaviors.ContainsKey(DefenseBehaviorName))
                return _defenseModifier = DefenseBehaviors[DefenseBehaviorName];
            return _attackModifier = DefaultBehavior;
        }
    } 

    private Func<int> _attackModifier;
    private Func<int> _defenseModifier;

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
        {"Dagger", () =>
        {
            int d = Dice.Roll(4);
            return d == 4 ? 5 : d; // Roll increases to a 5 on a critical hit.
        }
        }
    };

    public static readonly Dictionary<string, Func<int>> DefenseBehaviors = new Dictionary<string, Func<int>>
    {
        {"Buckler", () => Dice.Roll(4)}
    };
}