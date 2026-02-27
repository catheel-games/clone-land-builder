using UnityEngine;
using DG.Tweening;

public class LevelStartFeedbacks : MonoBehaviour
{
    [SerializeField] private FTUE _FTUE;
    [SerializeField] private CommentContainer _FTUECommentContainer;

    [SerializeField] private RectTransform starCircleRectTransform;
    [SerializeField] private RectTransform starCounterRectTransform;

    [SerializeField] private float starTargetX;
    [SerializeField] private float starStartX;

    private bool isFTUE = false;

    void Start()
    {
        isFTUE = !SaveLoadManager.Instance.gameData.progressData.ftueIsEnded;

        SetStartAnimations();
    }

    public void SetStartAnimations()
    {
        starCircleRectTransform.DOAnchorPosX(starStartX, 0);
        starCounterRectTransform.DOAnchorPosX(starStartX, 0);

        Sequence startSeq = DOTween.Sequence();

        startSeq.Join(starCircleRectTransform.DOAnchorPosX(starTargetX + 20, 0.5f))
                .Join(starCounterRectTransform.DOAnchorPosX(starTargetX + 20, 0.5f))
                .Append(starCircleRectTransform.DOAnchorPosX(starTargetX, 0.15f))
                .Join(starCounterRectTransform.DOAnchorPosX(starTargetX, 0.15f))
                .AppendCallback(() => _FTUECommentContainer.Show());

                if (isFTUE)
                {
                    startSeq.InsertCallback(0.25f, () => _FTUE.Setup());
                }

    }


}
