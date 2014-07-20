using System.Linq;
using UnityEngine;

public static class Dice
{
    public static int Roll(params int[] sides)
    {
        return sides.Sum(i => Random.Range(0, i) + 1);
    }
}