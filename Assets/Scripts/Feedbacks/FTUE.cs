using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FTUE : MonoBehaviour
{
    private bool fTUEisOn = false;
    private int fTUEtileID = 1;

    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private Vector3 secondTilePosition;
    [SerializeField] private Vector3 thirdTilePosition;

    [Header("Hand Refs")]
    [SerializeField] private DefaultContainer _FTUEPanelDefaultContainer;
    [SerializeField] private RectTransform handRectTransform;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Vector2 acceptPos;
    [SerializeField] private Vector2 tileSetPos;
    [SerializeField] private Vector2 rotatePos;
    [SerializeField] private GameObject rotateArrowsObject;

    [Header("Decline Button Refs")]
    [SerializeField] private Button declineButton;
    [SerializeField] private Image declineButtonImage;
    [SerializeField] private Sprite declineFtueSprite;
    [SerializeField] private Sprite declineDefaultSprite;

    public void Setup()
    {
        fTUEisOn = true;

        _FTUEPanelDefaultContainer.Show();
        handRectTransform.localPosition = tileSetPos;
    }

    public void SetTile()
    {
        if (fTUEisOn)
        {
            if (fTUEtileID == 1)
            {
                declineButton.enabled = false;
                declineButtonImage.sprite = declineFtueSprite;
                handRectTransform.localPosition = acceptPos;
            }
            else if (fTUEtileID == 2)
            {
                declineButton.enabled = true;
                declineButtonImage.sprite = declineDefaultSprite;
                handRectTransform.localPosition = rotatePos;

                handAnimator.SetTrigger("Rotate");
            }
        }
    }

    public void TileAccept()
    {
        if (fTUEisOn)
        {
            if (fTUEtileID == 1)
            {
                _FTUEPanelDefaultContainer.Hide();

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    cameraControl.LockPosition(secondTilePosition);
                    _FTUEPanelDefaultContainer.Show();
                    handRectTransform.localPosition = tileSetPos;
                });
            }
            else if (fTUEtileID == 2)
            {
                _FTUEPanelDefaultContainer.Hide();
                handRectTransform.gameObject.SetActive(false);

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    cameraControl.LockPosition(thirdTilePosition);
                    _FTUEPanelDefaultContainer.Show();
                    handRectTransform.localPosition = tileSetPos;
                });

            }

            fTUEtileID++;
        }
    }

}
