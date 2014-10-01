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
    Idle = 0,
    Attack = 1,
    Defend = -1,
    Hit = -2,
}