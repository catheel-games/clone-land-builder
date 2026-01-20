using System.Collections.Generic;
using UnityEngine;

public class TileGrid : Singleton<TileGrid>
{
    [SerializeField] private Transform tilePlacerContainerTransform;

    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TilePlacer tilePlacerPrefab;
    [SerializeField] private TilePreview tilePreviewPrefab;

    private TilePreview tilePreviewInstance;
    Hexagons.Coords tilePreviewCoords;

    private Dictionary<Hexagons.Coords, TilePlacer> frontier = new Dictionary<Hexagons.Coords, TilePlacer>();
    private Dictionary<Hexagons.Coords, Tile> tiles = new Dictionary<Hexagons.Coords, Tile>();

    void Start()
    {
        setTilePlacer(new Hexagons.Coords(0, 0), 0);
    }
    
    private void setTilePlacer(Hexagons.Coords coords, int i)
    {
        if (!frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer newTilePlacer = Instantiate(
                    tilePlacerPrefab,
                    Hexagons.HexToWorld(coords),
                    tilePlacerPrefab.transform.rotation,
                    tilePlacerContainerTransform
                );

                newTilePlacer.gameObject.name = $"asadasd {i}";

                newTilePlacer.Init(coords);

                frontier.Add(coords, newTilePlacer);
            }
        }
    }

    private void setTile(Hexagons.Coords coords)
    {
        if (frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                Destroy(frontier[coords].gameObject);

                frontier.Remove(coords);

                Tile newTile = tilePreviewInstance.PlaceTile();
                newTile.transform.position = Hexagons.HexToWorld(tilePreviewCoords);

                tiles.Add(coords, newTile);
                
                Hexagons.IterateNeighbours(coords, (int side, Hexagons.Coords neighbor) => {
                    setTilePlacer(neighbor, side);
                });
            }
        }
    }

    public Tile GetTile(Hexagons.Coords coords)
    {
        tiles.TryGetValue(coords, out Tile tile);
        return tile;
    }

    public void CreatePreviewTile(Hexagons.Coords coords)
    {
        tilePreviewInstance = Instantiate(
            tilePreviewPrefab,
            Hexagons.HexToWorld(coords),
            Quaternion.identity,
            transform
        );

        tilePreviewCoords = coords;

        tilePreviewInstance.Init(this, coords);

        LevelController.Instance.EnterTileViewMode(coords);
    }

    public void DeclinePreviewTile()
    {
        Destroy(tilePreviewInstance.gameObject);
        LevelController.Instance.ExitTileViewMode();

        tilePreviewInstance = null;
    }

    public void AcceptPreviewTile()
    {
        setTile(tilePreviewCoords);
        Destroy(tilePreviewInstance.gameObject);
        LevelController.Instance.ExitTileViewMode();

        tilePreviewInstance = null;
    }
}
