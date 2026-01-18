using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TilePlacementFeedback : MonoBehaviour
{
    private GameObject currentTileObject;

    [Header("Feedback Values")]
    [SerializeField] private float scaleTo1Duration = 0.1f;
    [SerializeField] private float punchScaleDuration = 0.2f;
    [SerializeField] private Vector3 punchScaleVector = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float hoveringValue = 0.075f;
    [SerializeField] private float hoveringDuration = 1.5f;

    public GameObject ActivateFeedback(GameObject currentTileObject)
    {
        Sequence activatingSeq = DOTween.Sequence();

        Sequence hoveringSeq = DOTween.Sequence();

        hoveringSeq
            .Append(currentTileObject.transform.DOMoveY(hoveringValue, hoveringDuration)
                                                .SetRelative().SetEase(Ease.InOutSine))
            .Append(currentTileObject.transform.DOMoveY(-hoveringValue, hoveringDuration)
                                                .SetRelative().SetEase(Ease.InOutSine))
            .SetLoops(-1);

        activatingSeq
            .Append(currentTileObject.transform.DOScale(0, 0))
            .Append(currentTileObject.transform.DOScale(1, scaleTo1Duration))
            .Append(currentTileObject.transform.DOPunchScale(punchScaleVector, punchScaleDuration, 0, 0))
            .Append(hoveringSeq);

        return currentTileObject;
    }

}
