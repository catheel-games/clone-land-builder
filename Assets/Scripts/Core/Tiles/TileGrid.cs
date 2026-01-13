using System.Collections.Generic;
using UnityEngine;

public class TileGrid : Singleton<TileGrid>
{
    [SerializeField] private Transform gridCanvasTransform;
    [SerializeField] private TilePlacer tilePlacerPrefab;
    [SerializeField] private GameObject tilePrefab;

    private Dictionary<Hexagons.Coords, TilePlacer> frontier = new Dictionary<Hexagons.Coords, TilePlacer>();
    private Dictionary<Hexagons.Coords, GameObject> tiles = new Dictionary<Hexagons.Coords, GameObject>();

    void Start()
    {
        setTilePlacer(new Hexagons.Coords(0, 0));
    }
    
    private void setTilePlacer(Hexagons.Coords coords)
    {
        if (!frontier.ContainsKey(coords))
        {
            TilePlacer newTilePlacer = Instantiate(
                tilePlacerPrefab,
                Hexagons.HexToWorld(coords),
                tilePlacerPrefab.transform.rotation,
                gridCanvasTransform
            );

            newTilePlacer.SetCoords(coords);

            frontier.Add(coords, newTilePlacer);
        }
    }

    public void SetTile(Hexagons.Coords coords)
    {
        if (frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer placerToRemove = frontier[coords];
                Destroy(placerToRemove.gameObject);
                frontier.Remove(coords);

                GameObject newTile = Instantiate(
                    tilePrefab,
                    Hexagons.HexToWorld(coords),
                    Quaternion.identity,
                    transform
                );

                tiles.Add(coords, newTile);

                
                Hexagons.IterateHexNeighbors(coords, (Hexagons.Coords neighbor) => {
                    setTilePlacer(neighbor);
                });
            }
        }
    }
}
