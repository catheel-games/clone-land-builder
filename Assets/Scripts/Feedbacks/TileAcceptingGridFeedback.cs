using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TileAcceptingGridFeedback : MonoBehaviour
{
    [SerializeField] private float scaleTo1Duration = 0.6f;

    public void Activate(Dictionary<Hexagons.Coords, TilePlacer> frontier)
    {
        foreach (KeyValuePair<Hexagons.Coords, TilePlacer> pair in frontier)
        {
            pair.Value.transform.DOScale(0, 0);
        }

        foreach (KeyValuePair<Hexagons.Coords, TilePlacer> pair in frontier)
        {
            pair.Value.transform.DOScale(1, scaleTo1Duration);
        }

    }
}
