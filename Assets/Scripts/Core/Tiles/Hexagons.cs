using UnityEngine;
using System;

public static class Hexagons
{
    const float sqrt3 = 1.73205080757f;
    const float sqrt3half = 0.86602540378f;

    public struct Coords
    {
        public int x;
        public int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static Vector3 HexToWorld(Coords hex)
    {
        return new Vector3(
            sqrt3 * hex.x + sqrt3half * Tools.Modulo(hex.y, 2),
            0f,
            1.5f * hex.y
        );
    }

    public static void IterateHexNeighbors(Coords hex, Action<Coords> action)
    {
        action(new Coords(hex.x - 1 + 2 * Tools.Modulo(hex.y, 2), hex.y));
        action(new Coords(hex.x + Tools.Modulo(hex.y, 2), hex.y + 1));
        action(new Coords(hex.x - (1 - Tools.Modulo(hex.y, 2)), hex.y + 1));
        action(new Coords(hex.x + 1 - 2 * Tools.Modulo(hex.y, 2), hex.y));
        action(new Coords(hex.x, hex.y - 1));
        action(new Coords(hex.x - 1 + 2 * Tools.Modulo(hex.y, 2), hex.y - 1));
    }
}
