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

    public Func<Hexagons.Type, (Hexagons.Type type, int count)> OnRequestLeastType;
    private int tilesPlaced = 0;
    private Hexagons.Type ignoredType = Hexagons.Type.Null;
    private int ignoreCounter = 0;

    void Start()
    {
        InitializeQueue();
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

    private Hexagons.Type GetRandomType()
    {
        return (Hexagons.Type)Random.Range(1, 6);
    } 

    private (Hexagons.Type type, int count) GetLeastOccurringType(Hexagons.Type excludeType = Hexagons.Type.Null)
    {
        return OnRequestLeastType?.Invoke(excludeType) ?? (Hexagons.Type.Grass, 0);
    }

    private Hexagons.Type GetRandomTypeExcluding(Hexagons.Type firstType)
    {
        Hexagons.Type secondType = GetRandomType();
        if (secondType == firstType)
            return GetRandomTypeExcluding(firstType);
        return secondType;
    }

    private Tile GetUniqueTile(Tile[] arr, List<Tile> usedTiles)
    {
        Tile picked;

        do
        {
            picked = arr[Random.Range(0, arr.Length)];
        }
        while (usedTiles.Contains(picked));

        return picked;
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
        Tile typed1 = GetTypeTile(type);
        Tile typed2 = GetTypeTile(type);
        while (typed2 == typed1)
        {
            typed2 = GetTypeTile(type);
        }
        newSet.Add(typed1);
        newSet.Add(typed2);
        newSet = newSet.OrderBy(x => Random.value).ToList();
        return newSet;
    }

    private List<Tile> CreateRandomSet(Hexagons.Type type)
    {
        List<Tile> randomSet = new List<Tile>();
        randomSet.Add(GetFullTile(type));
        Tile typed1 = GetTypeTile(type);
        Tile typed2 = GetTypeTile(type);
        while (typed2 == typed1)
        {
            typed2 = GetTypeTile(type);
        }
        randomSet.Add(typed1);
        randomSet.Add(typed2);

        randomSet = randomSet.OrderBy(x => Random.value).ToList();

        int rand = Random.Range(0, 100);
        if (rand >= 85)
        {
            Tile random1 = GetRandomTile();
            Tile random2 = GetRandomTile();
            while (random2 == random1)
            {
                random2 = GetRandomTile();
            }
            randomSet.Add(random1);
            randomSet.Add(random2);
        }
        else if (rand >= 70)
        {
            randomSet.Add(GetRandomTile());
        }

        return randomSet;
    }

    private void RefillQueue()
    {
        if (setQueue.Count < 3)
        {
            var leastType = GetLeastOccurringType();

            if (tilesPlaced >= 15 && leastType.count <= 2 && ignoreCounter == 0)
            {
                ignoredType = leastType.type;
                ignoreCounter = 8;
            }

            Hexagons.Type typeToUse;
            if(ignoreCounter > 0 && leastType.type == ignoredType)
            {
                typeToUse = GetLeastOccurringType(ignoredType).type;
            }
            else
            {
                typeToUse = leastType.type;
            }

            setQueue.AddRange(CreateRandomSet(typeToUse));
        }
    }

    public Tile ShowNextTile()
    {
        return setQueue[0];
    }

    public Tile GetNextTile()
    {
        Tile prevTile = setQueue[0];
        setQueue.RemoveAt(0);

        tilesPlaced++;
        if (ignoreCounter > 0) ignoreCounter--;

        RefillQueue();
        return prevTile;
    }

    public List<Tile> GetQueue()
    {
        return setQueue;
    }
}