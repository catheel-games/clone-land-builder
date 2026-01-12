using UnityEngine;
using DG.Tweening;

public class TileDenyingFeedback : MonoBehaviour
{

    [Header("Feedback Values")]
    [SerializeField] private float scaleTo0Duration = 0.5f;
    [SerializeField] private float scaleTo1Duration = 0.6f;

    public void ActivateFeedback(GameObject currentTileObject, GameObject[] tilePlacementZones)
    {
        Sequence activatingSeq = DOTween.Sequence();

        activatingSeq       
            .AppendCallback(() =>
            {
                for (int i = 0; i < tilePlacementZones.Length; i++)
                {
                    tilePlacementZones[i].transform.DOScale(1, scaleTo1Duration);
                }
            })
            .Join(currentTileObject.transform.DOScale(0, scaleTo0Duration))
            .OnComplete(() => Destroy(currentTileObject));

    }
}
