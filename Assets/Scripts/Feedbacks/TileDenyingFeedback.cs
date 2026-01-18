using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TileDenyingFeedback : MonoBehaviour
{

    [Header("Feedback Values")]
    [SerializeField] private float scaleTo0Duration = 0.5f;
    [SerializeField] private float scaleTo1Duration = 0.6f;

    public void ActivateFeedback(GameObject currentTileObject, Dictionary<Hexagons.Coords, TilePlacer> frontier)
    {
        foreach (KeyValuePair<Hexagons.Coords, TilePlacer> pair in frontier)
        {
            pair.Value.transform.DOScale(0, 0);
        }

        Sequence activatingSeq = DOTween.Sequence();

        activatingSeq       
            .AppendCallback(() =>
            {
                foreach (KeyValuePair<Hexagons.Coords, TilePlacer> pair in frontier)
                {
                    pair.Value.transform.DOScale(1, scaleTo1Duration);
                }
            })
            .Join(currentTileObject.transform.DOScale(0, scaleTo0Duration))
            .OnComplete(() => Destroy(currentTileObject));

    }
}
