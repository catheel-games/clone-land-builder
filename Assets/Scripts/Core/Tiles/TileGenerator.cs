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

    List<Tile> displayedTiles = new List<Tile>();

    void Start()
    {
        displayedTiles.Add(GetRandomTile());
        displayedTiles.Add(GetRandomTile());
        displayedTiles.Add(GetRandomTile());
    }

    private Tile GetRandomTile()
    {
        int randomNum = UnityEngine.Random.Range(0, tiles.Length);
        return tiles[randomNum];
    }

    public Tile ShowNextTile()
    {
        return displayedTiles[0];
    }

    public Tile GetNextTile()
    {
        Tile prevTile = displayedTiles[0];
        displayedTiles.RemoveAt(0);
        displayedTiles.Add(GetRandomTile());

        return prevTile;
    }
}