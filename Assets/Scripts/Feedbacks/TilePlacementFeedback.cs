using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TilePlacementFeedback : MonoBehaviour
{

    private Transform currentTileTransform;
    private Coroutine hoveringCoroutine;

    [Header("Feedback Values")]
    [SerializeField] private float scaleTo1Duration = 0.1f;
    [SerializeField] private float punchScaleDuration = 0.2f;
    [SerializeField] private Vector3 punchScaleVector = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float hoveringValue = 0.075f;
    [SerializeField] private float hoveringDuration = 1.5f;

    public Transform Activate(Transform currentTileObject)
    {
        currentTileTransform = currentTileObject;

        Sequence activatingSeq = DOTween.Sequence();

        activatingSeq
            .Append(currentTileObject.DOScale(0, 0))
            .Append(currentTileObject.DOScale(1, scaleTo1Duration))
            .Append(currentTileObject.DOPunchScale(punchScaleVector, punchScaleDuration, 0, 0))
            .AppendCallback(() => hoveringCoroutine = StartCoroutine(Hovering()));

        return currentTileObject;
    }



    public void StopHovering()
    {
        StopCoroutine(hoveringCoroutine);
        hoveringCoroutine = null;
        currentTileTransform.DOKill();

        Debug.Log("Hovering Stopped");
    }

    IEnumerator Hovering()
    {
        currentTileTransform.DOMoveY(hoveringValue, hoveringDuration)
                                            .SetRelative().SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(hoveringDuration);

        currentTileTransform.DOMoveY(-hoveringValue, hoveringDuration)
                                                .SetRelative().SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(hoveringDuration);
        StartCoroutine(Hovering());
    }

}
