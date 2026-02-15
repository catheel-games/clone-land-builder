using UnityEngine;
using DG.Tweening;

public class LevelLoseFeedback : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private CanvasGroup exitToMenuCanvasGroup;

    public void LevelLosing()
    {
        Sequence winSeq = DOTween.Sequence();

        winSeq.AppendCallback(() => InputManager.Instance.DisableInput())
              .AppendCallback(levelCanvasControl.DisableWinContainers)
              .AppendCallback(() => levelCanvasControl._levelTopPartContainer.Hide())
              .AppendCallback(() => levelCanvasControl._coinSectionContainer.Hide())
              .AppendInterval(0.2f)

              .AppendCallback(() => AudioManager.Instance.PlaySound("Other", "Level Fail"))
              .Append(exitToMenuCanvasGroup.DOFade(0, 0))
              .AppendCallback(() => levelCanvasControl._levelFailContainer.Show())
              .AppendCallback(() => SaveLoadManager.Instance.RefreshLevelData())
              .AppendCallback(() => LevelControl.Instance.CameraPresentation())

              .AppendInterval(1f)
              .Append(exitToMenuCanvasGroup.DOFade(1, 0.2f));

    }
}
