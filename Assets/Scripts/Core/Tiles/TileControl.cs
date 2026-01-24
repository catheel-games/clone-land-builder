using System;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private TilePreview tilePreview;
    [SerializeField] private TileGenerator tileGenerator;

    private Tile previewTile;
    private Hexagons.Coords previewTileCoords;

    public event Action<Hexagons.Coords> OnTilePlacerClick;
    public event Action<List<Tile>> OnTileGeneratorUpdate;

    void OnEnable()
    {
        tileGrid.OnTilePlacerClick += EnterTilePreview;   
    }

    void OnDisable()
    {
        tileGrid.OnTilePlacerClick -= EnterTilePreview;   
    }

    void Start()
    {
        tileGenerator.InitializeQueue();
        OnTileGeneratorUpdate?.Invoke(tileGenerator.GetQueue());
    }

    private void EnterTilePreview(Hexagons.Coords coords)
    {
        previewTile = Instantiate(tileGenerator.ShowNextTile(), transform);
        previewTileCoords = coords;

        OnTilePlacerClick?.Invoke(coords);
        tilePreview.StartPreviewAtCoords(previewTile, coords);
    }

    public void ExitTilePreview(bool isTileAccepted)
    {
        if (isTileAccepted)
        {
            tileGrid.SetTile(previewTileCoords, previewTile);
            tileGenerator.GetNextTile();
            OnTileGeneratorUpdate?.Invoke(tileGenerator.GetQueue());
        }

        tilePreview.EndPreview(isTileAccepted);
        tileGrid.Unlock();
    }
}
