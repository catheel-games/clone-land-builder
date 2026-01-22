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
