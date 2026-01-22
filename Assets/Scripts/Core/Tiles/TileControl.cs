using System;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private TilePreview tilePreview;
    [SerializeField] private TileGenerator tileGenerator;

    private Tile previewTile;
    private Hexagons.Coords previewTileCoords;

    public event Action<Hexagons.Coords> OnTilePlacerClick;

    void OnEnable()
    {
        tileGrid.OnTilePlacerClick += EnterTilePreview;   
    }

    void OnDisable()
    {
        tileGrid.OnTilePlacerClick -= EnterTilePreview;   
    }

    private void EnterTilePreview(Hexagons.Coords coords)
    {
        previewTile = tileGenerator.GetRandomTile();
        previewTileCoords = coords;

        OnTilePlacerClick?.Invoke(coords);
        tilePreview.StartPreviewAtCoords(previewTile, coords);
    }

    public void ExitTilePreview(bool isTileAccepted)
    {
        if (isTileAccepted)
        {
            tileGrid.SetTile(previewTileCoords, previewTile);
        }

        tilePreview.EndPreview(isTileAccepted);
        tileGrid.Unlock();
    }
}
