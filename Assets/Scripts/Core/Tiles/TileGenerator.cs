using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class TileGenerator : Singleton<TileGenerator>
{
    [System.Serializable]
    public class FullTiles
    {
        public Tile[] fullGrassTiles;
        public Tile[] fullWaterTiles;
        public Tile[] fullTownTiles;
        public Tile[] fullYellowTiles;
        public Tile[] fullForestTiles;
    }

    [System.Serializable]
    public class TypeTiles
    {
        public Tile[] grassTiles;
        public Tile[] waterTiles;
        public Tile[] townTiles;
        public Tile[] yellowTiles;
        public Tile[] forestTiles;
    }

    // [SerializeField] private Tile[] tiles;
    [SerializeField] private FullTiles fullTiles;
    [SerializeField] private TypeTiles typeTiles; 

    List<Tile> setQueue = new List<Tile>();

    public Func<Hexagons.Type> OnRequestLeastType;

    void Start()
    {
        InitializeQueue();
    }

    private Hexagons.Type GetRandomType()
    {
        return (Hexagons.Type)Random.Range(1, 6);
    } 

    private Hexagons.Type GetLeastOccurringType()
    {
        return OnRequestLeastType?.Invoke() ?? Hexagons.Type.Grass;
    }

    private Hexagons.Type GetRandomTypeExcluding(Hexagons.Type firstType)
    {
        Hexagons.Type secondType = GetRandomType();
        if (secondType == firstType)
            return GetRandomTypeExcluding(firstType);
        return secondType;
    }

    private Tile GetFullTile(Hexagons.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Hexagons.Type.Grass:
                arr = fullTiles.fullGrassTiles;
                break;
            case Hexagons.Type.Water:
                arr = fullTiles.fullWaterTiles;
                break;
            case Hexagons.Type.Town:
                arr = fullTiles.fullTownTiles;
                break;
            case Hexagons.Type.Yellow:
                arr = fullTiles.fullYellowTiles;
                break;
            case Hexagons.Type.Forest:
                arr = fullTiles.fullForestTiles;
                break;
            default:
                arr = fullTiles.fullGrassTiles;
                break;
        }

        if (arr.Length == 0)
            return GetFullTile(GetRandomType());

        return arr[Random.Range(0, arr.Length)];
    }

    private Tile GetTypeTile(Hexagons.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Hexagons.Type.Grass:
                arr = typeTiles.grassTiles;
                break;
            case Hexagons.Type.Water:
                arr = typeTiles.waterTiles;
                break;
            case Hexagons.Type.Town:
                arr = typeTiles.townTiles;
                break;
            case Hexagons.Type.Yellow:
                arr = typeTiles.yellowTiles;
                break;
            case Hexagons.Type.Forest:
                arr = typeTiles.forestTiles;
                break;
            default:
                arr = typeTiles.grassTiles;
                break;
        }

        if (arr.Length == 0)
            return GetTypeTile(GetRandomType());

        return arr[Random.Range(0, arr.Length)];
    }

    private List<Tile> CreateTypedSet(Hexagons.Type type)
    {
        List<Tile> newSet = new List<Tile>();
        newSet.Add(GetFullTile(type));
        newSet.Add(GetTypeTile(type));
        newSet.Add(GetTypeTile(type));
        newSet = newSet.OrderBy(x => Random.value).ToList();
        return newSet;
    }

    private List<Tile> CreateRandomSet(Hexagons.Type type)
    {
        List<Tile> randomSet = new List<Tile>();
        randomSet.Add(GetFullTile(type));
        randomSet.Add(GetTypeTile(type));
        randomSet.Add(GetTypeTile(type));

        randomSet = randomSet.OrderBy(x => Random.value).ToList();

        int rand = Random.Range(0, 100);
        if (rand >= 85)
        {
            randomSet.Add(GetRandomTile());
            randomSet.Add(GetRandomTile());
        }
        else if (rand >= 70)
        {
            randomSet.Add(GetRandomTile());
        }

        return randomSet;
    }

    private void RefillQueue(Hexagons.Type type)
    {
        if (setQueue.Count < 3)
        {
            setQueue.AddRange(CreateRandomSet(type));
        }
    }

    private void InitializeQueue()
    {
        Hexagons.Type type1 = GetRandomType();
        Hexagons.Type type2 = GetRandomTypeExcluding(type1);
        setQueue.AddRange(CreateTypedSet(type1));
        setQueue.AddRange(CreateTypedSet(type2));
    }

    private Tile GetRandomTile()
    {
        Hexagons.Type randomType = GetRandomType();
        Tile randomTile = Random.Range(0, 2) == 0 ? GetFullTile(randomType) : GetTypeTile(randomType);
        return randomTile;
    }

    public Tile ShowNextTile()
    {
        return setQueue[0];
    }

    public Tile GetNextTile()
    {
        Tile prevTile = setQueue[0];
        setQueue.RemoveAt(0);
        RefillQueue(GetLeastOccurringType());
        return prevTile;
    }

    public List<Tile> GetQueue()
    {
        return setQueue;
    }
}
