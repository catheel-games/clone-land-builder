using TMPro;
using UnityEngine;

public class TilePreviewCombo : MonoBehaviour
{
    [SerializeField] private TMP_Text combinationNumberText;
    [SerializeField] private Transform visual;
    [SerializeField] private TilePreviewComboPullInFeedback pullInFeedback;

    private int combinationNumber;

    public void Setup(int combinationNumber, TilePreviewStar[] stars)
    {
        this.combinationNumber = combinationNumber;
        combinationNumberText.text = "" + combinationNumber;

        pullInFeedback.Play(visual, stars);
    }

    public void Kill()
    {
        pullInFeedback.Kill();
    }
}
