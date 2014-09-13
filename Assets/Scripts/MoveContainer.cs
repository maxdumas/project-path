using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MoveContainer
{
    public float Delay;
    public MoveType MoveType;
}

public enum MoveType
{
    Attack,
    Defend,
    Idle
}