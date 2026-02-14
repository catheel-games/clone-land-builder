using UnityEngine;
using DG.Tweening;

public class LevelLoseFeedback : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;

    public void LevelLosing()
    {
        Debug.Log("Level Losing");

        Sequence winSeq = DOTween.Sequence();

        winSeq.AppendCallback(() => InputManager.Instance.DisableInput())
              .AppendCallback(levelCanvasControl.DisableWinContainers)
              .AppendCallback(() => levelCanvasControl._levelTopPartContainer.Hide())
              .AppendCallback(() => levelCanvasControl._coinSectionContainer.Hide())
              .AppendInterval(0.2f)
              .AppendCallback(() => AudioManager.Instance.PlaySound("Other", "Level Fail"))
              .AppendCallback(() => levelCanvasControl._levelFailContainer.Show());
    }
}
