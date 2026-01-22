using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// using System.Diagnostics;

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

    void Start()
    {
        InitializeQueue();
    }

    // change to least occuring type
    private Tile.Type GetRandomType()
    {
        return (Tile.Type)UnityEngine.Random.Range(1, 6);
    } 

    private Tile.Type GetRandomTypeExcluding(Tile.Type firstType)
    {
        Tile.Type secondType = GetRandomType();
        if (secondType == firstType)
            return GetRandomTypeExcluding(firstType);
        return secondType;
    }

    private Tile GetFullTile(Tile.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Tile.Type.Grass:
                arr = fullTiles.fullGrassTiles;
                break;
            case Tile.Type.Water:
                arr = fullTiles.fullWaterTiles;
                break;
            case Tile.Type.Town:
                arr = fullTiles.fullTownTiles;
                break;
            case Tile.Type.Yellow:
                arr = fullTiles.fullYellowTiles;
                break;
            case Tile.Type.Forest:
                arr = fullTiles.fullForestTiles;
                break;
            default: 
                arr = fullTiles.fullGrassTiles;
                break;
        }

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    private Tile GetTypeTile(Tile.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Tile.Type.Grass:
                arr = typeTiles.grassTiles;
                break;
            case Tile.Type.Water:
                arr = typeTiles.waterTiles;
                break;
            case Tile.Type.Town:
                arr = typeTiles.townTiles;
                break;
            case Tile.Type.Yellow:
                arr = typeTiles.yellowTiles;
                break;
            case Tile.Type.Forest:
                arr = typeTiles.forestTiles;
                break;
            default:
                arr = typeTiles.grassTiles;
                break;
        }

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    private List<Tile> CreateTypedSet(Tile.Type type)
    {
        List<Tile> newSet = new List<Tile>();
        newSet.Add(GetFullTile(type));
        newSet.Add(GetTypeTile(type));
        newSet.Add(GetTypeTile(type));
        newSet = newSet.OrderBy(x => Random.value).ToList();
        return newSet;
    }

    // will not be so random after least occuring edge is implemented
    private List<Tile> CreateRandomSet(Tile.Type type)
    {
        List<Tile> randomSet = new List<Tile>();
        randomSet.Add(Random.Range(0, 2) == 0 ? GetFullTile(type) : GetTypeTile(type));
        randomSet.Add(Random.Range(0, 2) == 0 ? GetFullTile(type) : GetTypeTile(type));
        randomSet.Add(Random.Range(0, 2) == 0 ? GetFullTile(type) : GetTypeTile(type));

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

    private void RefillQueue(Tile.Type type)
    {
        if (setQueue.Count < 3)
        {
            setQueue.AddRange(CreateRandomSet(type));
        } 
    }

    private void InitializeQueue()
    {
        Tile.Type type1 = GetRandomType();
        Tile.Type type2 = GetRandomTypeExcluding(type1);
        setQueue.AddRange(CreateTypedSet(type1));
        setQueue.AddRange(CreateTypedSet(type2));
    }

    private Tile GetRandomTile()
    {
        Tile.Type randomType = GetRandomType();
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
        RefillQueue(GetRandomType());
        return prevTile;
    }

    public List<Tile> GetQueue()
    {
        return setQueue;
    }
}
