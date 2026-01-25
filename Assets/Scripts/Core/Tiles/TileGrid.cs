using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private UIContainer tilePlacerContainer;
    [SerializeField] private TilePlacer tilePlacerPrefab;

    private Dictionary<Hexagons.Coords, TilePlacer> frontier = new Dictionary<Hexagons.Coords, TilePlacer>();
    private Dictionary<Hexagons.Coords, Tile> tiles = new Dictionary<Hexagons.Coords, Tile>();

    public event Action<Hexagons.Coords> OnTilePlacerClick;

    void Start()
    {
        SetTilePlacer(new Hexagons.Coords(0, 0));
    }
    
    private void SetTilePlacer(Hexagons.Coords coords)
    {
        if (!frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer newTilePlacer = Instantiate(tilePlacerPrefab, tilePlacerContainer.transform);
                newTilePlacer.Setup(coords);
                newTilePlacer.OnClick += Lock;
                frontier.Add(coords, newTilePlacer);
            }
        }
    }

    public void SetTile(Hexagons.Coords coords, Tile tile)
    {
        if (frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer oldTilePlacer = frontier[coords];
                oldTilePlacer.OnClick -= Lock;
                Destroy(oldTilePlacer.gameObject);
                frontier.Remove(coords);

                tile.transform.SetParent(transform, false);
                tile.transform.position = Hexagons.HexToWorld(coords);
                tiles.Add(coords, tile);
                
                Hexagons.IterateNeighbours(coords, (int side, Hexagons.Coords neighbor) => {
                    SetTilePlacer(neighbor);
                });
            }
        }
    }
    
    public Tile GetTile(Hexagons.Coords coords)
    {
        tiles.TryGetValue(coords, out Tile tile);
        return tile;
    }

    public (Hexagons.Type type, int count) GetLeastOccurringSide(Hexagons.Type excludeType = Hexagons.Type.Null)
    {
        int[] sideCounts = new int[6];
        int smallestCount = int.MaxValue;
        int smallestIndex = 1;

        foreach (Hexagons.Coords frontierCoord in frontier.Keys)
        {
            Hexagons.IterateNeighbours(frontierCoord, (int side, Hexagons.Coords neighbor) => {
                if(tiles.ContainsKey(neighbor))
                {
                    Tile neighborTile = GetTile(neighbor);
                    if (neighborTile != null)
                    {
                        Hexagons.Type neighborType = neighborTile.GetSide(Tools.Modulo(side + 3, 6));
                        sideCounts[(int)neighborType]++;
                    }
                }
            });
        }

        for (int i = 1; i < sideCounts.Length; i++)
        {
            if ((Hexagons.Type)i == excludeType) continue;

            if (sideCounts[i] < smallestCount)
            {
                smallestCount = sideCounts[i];
                smallestIndex = i;
            }
        }

        return ((Hexagons.Type)smallestIndex, smallestCount);
    }

    private void Lock(Hexagons.Coords coords)
    {
        tilePlacerContainer.Hide();
        OnTilePlacerClick?.Invoke(coords);
    }

    public void Unlock()
    {
        tilePlacerContainer.Show();
    }
}