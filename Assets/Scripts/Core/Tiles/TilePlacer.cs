using System;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private RectTransform tilePlacerPlus;

    private Hexagons.Coords coords;

    void LateUpdate()
    {
        tilePlacerPlus.localRotation = Quaternion.Euler(0f, 0f, -LevelController.Instance.CameraPivotRotation);
    }

    public void Init(Hexagons.Coords coords)
    {
        this.coords = coords;
    }

    public void Click()
    {
        TileGrid.Instance.CreatePreviewTile(coords);
    }
}
