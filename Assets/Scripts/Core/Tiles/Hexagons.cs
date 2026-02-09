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

    public enum Type
    {
        Null,
        Town,
        Grass,
        Forest,
        Yellow,
        Water    
    }

    public static Vector3 HexToWorld(Coords hex)
    {
        return new Vector3(
            sqrt3 * hex.x + sqrt3half * Tools.Modulo(hex.y, 2),
            0f,
            1.5f * hex.y
        );
    }

    public static Coords WorldToHex(Vector3 world)
    {
        int y = Mathf.RoundToInt(world.z / 1.5f);
        int x = Mathf.RoundToInt((world.x - sqrt3half * Tools.Modulo(y, 2)) / sqrt3);
        return new Coords(x, y);
    }

    public static void IterateNeighbours(Coords hex, Action<int, Coords> action)
    {
        action(0, new Coords(hex.x + Tools.Modulo(hex.y, 2), hex.y + 1));
        action(1, new Coords(hex.x + 1, hex.y));
        action(2, new Coords(hex.x + Tools.Modulo(hex.y, 2), hex.y - 1));
        action(3, new Coords(hex.x - 1 + Tools.Modulo(hex.y, 2), hex.y - 1));
        action(4, new Coords(hex.x - 1, hex.y));
        action(5, new Coords(hex.x - 1 + Tools.Modulo(hex.y, 2), hex.y + 1));
    }
}
    