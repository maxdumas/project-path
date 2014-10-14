using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will hold all data for the player, i.e. their health, their items, etc.
/// </summary>
public class Player : Actor
{
    public PathFollower PathFollower;

    public override void OnEnable()
    {
        if (PathFollower == null) PathFollower = GetComponent<PathFollower>();

        base.OnEnable();
    }
}
