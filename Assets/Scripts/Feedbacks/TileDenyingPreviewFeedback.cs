using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TileDenyingPreviewFeedback : MonoBehaviour
{

    [Header("Feedback Values")]
    [SerializeField] private float scaleTo0Duration = 0.5f;
    [SerializeField] private float scaleTo1Duration = 0.6f;

    public void Activate(GameObject currentTileObject)
    {
        Sequence activatingSeq = DOTween.Sequence();

        activatingSeq       
            .Join(currentTileObject.transform.DOScale(0, scaleTo0Duration))
            .OnComplete(() => Destroy(currentTileObject));

    }
}
