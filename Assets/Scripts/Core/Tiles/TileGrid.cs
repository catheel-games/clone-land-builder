using System.Collections.Generic;
using UnityEngine;

public class TileGridController : Singleton<TileGridController>
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
        setTilePlacer(new Hexagons.Coords(0, 0));
    }
    
    private void setTilePlacer(Hexagons.Coords coords)
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

                Tile newTile = Instantiate(
                    TileGenerator.Instance.GetNextTile(),
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

    public void CreatePreviewTile(Hexagons.Coords coords)
    {
        tilePreviewInstance = Instantiate(
            tilePreviewPrefab,
            Hexagons.HexToWorld(coords),
            Quaternion.identity,
            transform
        );

        Tile nextTilePrefab = TileGenerator.Instance.ShowNextTile();
        Tile spawnedTile = Instantiate(nextTilePrefab, tilePreviewInstance.transform);
        tilePreviewInstance.SetTile(spawnedTile);

        TilePreviewInputInterpreter.Instance.SetTilePreview(tilePreviewInstance);

        tilePreviewCoords = coords;

        LevelController.Instance.EnterTileViewMode(coords);
    }

    public void DeclinePreviewTile()
    {
        Destroy(tilePreviewInstance.gameObject);
        LevelController.Instance.ExitTileViewMode();
    }

    public void AcceptPreviewTile()
    {
        Destroy(tilePreviewInstance.gameObject);
        setTile(tilePreviewCoords);
        LevelController.Instance.ExitTileViewMode();
    }
}
