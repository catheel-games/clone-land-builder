using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : Singleton<TileGenerator>
{
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