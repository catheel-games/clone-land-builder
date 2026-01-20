using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField] private Tile[] tiles;
    [SerializeField] private FullTiles fullTiles;
    [SerializeField] private TypeTiles typeTiles; 

    List<Tile> set = new List<Tile>();

    void Start()
    {
        GenerateStartingSets();
    }

    private Tile GetRandomTile()
    {
        int randomNum = UnityEngine.Random.Range(0, tiles.Length);
        return tiles[randomNum];
    }

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

    private Tile GetFullTileOfType(Tile.Type type)
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

    private Tile GetTypeTileOfType(Tile.Type type)
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

    private List<Tile> GenerateSet(Tile.Type type)
    {
        List<Tile> newSet = new List<Tile>();
        newSet.Add(GetFullTileOfType(type));
        newSet.Add(GetTypeTileOfType(type));
        newSet.Add(GetTypeTileOfType(type));
        return newSet;
    }

    private void GenerateStartingSets()
    {
        Tile.Type type1 = GetRandomType();
        Tile.Type type2 = GetRandomTypeExcluding(type1);
        set.AddRange(GenerateSet(type1));
        set.AddRange(GenerateSet(type2));
        Debug.Log("Queue has " + set.Count + " tiles!");
    }

    public Tile ShowNextTile()
    {
        return set[0];
    }

    public Tile GetNextTile()
    {
        Tile prevTile = set[0];
        set.RemoveAt(0);
        return prevTile;
    }
}