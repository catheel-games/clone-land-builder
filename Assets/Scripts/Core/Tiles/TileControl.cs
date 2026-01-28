using System;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private TilePreview tilePreview;
    [SerializeField] private TileUpgrade tileUpgrade;
    [SerializeField] private TileGenerator tileGenerator;

    private Tile previewTile;
    private Hexagons.Coords previewTileCoords;

    public event Action<Hexagons.Coords> OnTilePlacerClick;
    public event Action<List<Tile>> OnTileGeneratorUpdate;
    public event Func<Tile> OnTileQueueFirstTileRequest;

    void OnEnable()
    {
        tileGrid.OnTilePlacerClick += EnterTilePreview;
        tileGenerator.OnLeastTypeRequest += tileGrid.GetLeastOccurringSide;
    }

    void OnDisable()
    {
        tileGrid.OnTilePlacerClick -= EnterTilePreview;
        tileGenerator.OnLeastTypeRequest -= tileGrid.GetLeastOccurringSide;
    }

    void Start()
    {
        tileGenerator.InitializeQueue();
        OnTileGeneratorUpdate?.Invoke(tileGenerator.GetQueue());
    }

    private void EnterTilePreview(Hexagons.Coords coords)
    {
        Tile tileUI = OnTileQueueFirstTileRequest?.Invoke();

        previewTile = Instantiate(tileGenerator.ShowNextTile(), transform);
        previewTileCoords = coords;

        previewTile.RotateInstantly(tileUI.RotationOffsetDiscrete);

        OnTilePlacerClick?.Invoke(coords);
        tilePreview.StartPreviewAtCoords(previewTile, tileUI, coords);
    }

    public void ExitTilePreview(bool isTileAccepted)
    {
        if (isTileAccepted)
        {
            tileGrid.SetTile(previewTileCoords, previewTile);
            tileUpgrade.CheckUpgrade(previewTileCoords);
            tileGenerator.GetNextTile();
            OnTileGeneratorUpdate?.Invoke(tileGenerator.GetQueue());
        }

        tilePreview.EndPreview(isTileAccepted);
        tileGrid.Unlock();
    }
}
