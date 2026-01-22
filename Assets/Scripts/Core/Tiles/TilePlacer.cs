using System;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private RectTransform tilePlacerPlus;

    private Hexagons.Coords coords;

    public event Action<Hexagons.Coords> OnClick;

    void LateUpdate()
    {
        tilePlacerPlus.localRotation = Quaternion.Euler(0f, 0f, -LevelControl.Instance.CameraPivotRotation);
    }

    public void Setup(Hexagons.Coords coords)
    {
        this.coords = coords;
        
        transform.position = Hexagons.HexToWorld(coords);
    }

    public void Click()
    {
        OnClick?.Invoke(coords);
    }
}
