using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FTUE : MonoBehaviour
{
    private bool fTUEisOn = false;

    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private Vector3[] tilePositions;
    [SerializeField] private TilePreviewControlContainer tilePreviewControlContainer;
    [SerializeField] private CameraControlContainer cameraControlContainer;

    [Header("Olivia Texts")]
    private Material uiCircleHoleMat;
    [SerializeField] private Image dialoguePanelImage;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject[] oliviaDialogues;

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

    private void Start()
    {
        uiCircleHoleMat = Instantiate(dialoguePanelImage.material);
        dialoguePanelImage.material = uiCircleHoleMat;
    }

    public void Setup()
    {
        fTUEisOn = true;

        _FTUEPanelDefaultContainer.Show();
        handRectTransform.localPosition = tileSetPos;

        cameraControl.LockPosition(tilePositions[0]);
    }

    public void SetTile()
    {
        if (fTUEisOn)
        {
            if (LevelControl.Instance.fTUEtileID == 0)
            {
                declineButton.enabled = false;
                declineButtonImage.sprite = declineFtueSprite;
                handRectTransform.localPosition = acceptPos;

                InputManager.Instance.DisableInput();
            }
            else if (LevelControl.Instance.fTUEtileID == 1)
            {
                declineButton.enabled = true;
                declineButtonImage.sprite = declineDefaultSprite;
                handRectTransform.localPosition = rotatePos;

                handAnimator.SetTrigger("Rotate");
                DOVirtual.DelayedCall(0.02f, () => tilePreviewControlContainer.Hide());
            }
            else if (LevelControl.Instance.fTUEtileID == 2)
            {
                _FTUEPanelDefaultContainer.Show();
                SetOliviaText(0);
                MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.475f), 0f);
                AnimateCircleHoleRadius(0.5f, 0.08f, 0.2f);

                DOVirtual.DelayedCall(0.02f, () => cameraControlContainer.Hide());
            }
        }
    }

    public void TileAccept(bool isTileAccepted)
    {
        if (fTUEisOn)
        {
            if (!isTileAccepted)
            {
                _FTUEPanelDefaultContainer.Hide();

                if (LevelControl.Instance.fTUEtileID == 1)
                {
                    DOVirtual.DelayedCall(0.2f, () =>
                    {
                        cameraControl.LockPosition(tilePositions[1]);
                        _FTUEPanelDefaultContainer.Show();

                        handRectTransform.localPosition = tileSetPos;
                        handAnimator.SetTrigger("Float");
                    });
                }

                else if (LevelControl.Instance.fTUEtileID == 2)
                {
                    DOVirtual.DelayedCall(0.2f, () =>
                    {
                        cameraControl.LockPosition(tilePositions[2]);
                        cameraControlContainer.Show();
                    });
                }
            }

            else if (isTileAccepted)
            {
                _FTUEPanelDefaultContainer.Hide();

                LevelControl.Instance.fTUEtileID++;


                if (LevelControl.Instance.fTUEtileID == 1)
                {
                    InputManager.Instance.EnableInput();

                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        cameraControl.LockPosition(tilePositions[1]);
                        _FTUEPanelDefaultContainer.Show();
                        handRectTransform.localPosition = tileSetPos;
                    });
                }
                else if (LevelControl.Instance.fTUEtileID == 2)
                {
                    handAnimator.gameObject.SetActive(false);

                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        cameraControl.LockPosition(tilePositions[2]);
                        _FTUEPanelDefaultContainer.Show();
                        handRectTransform.localPosition = tileSetPos;
                    });

                }
                else if (LevelControl.Instance.fTUEtileID == 3)
                {
                    _FTUEPanelDefaultContainer.Show();
                    cameraControlContainer.Show();

                    SetOliviaText(1);
                    MoveCircleHoleCenter(new Vector2(0.5f, 0.475f), new Vector2(0.115f, 0.8f), 0f);
                    AnimateCircleHoleRadius(0.5f, 0.045f, 0.2f);

                    GameData.Instance.ftueIsEnded = true;
                    fTUEisOn = false;
                    SaveLoadManager.Instance.SaveData();
                    LevelControl.Instance.EndingFTUEGrid();
                }
            }
        }
    }

    public void TileRotate() {
        if (fTUEisOn)
        {
            if (LevelControl.Instance.fTUEtileID == 1)
            {
                _FTUEPanelDefaultContainer.Hide();
                tilePreviewControlContainer.Show();
            }
        }
    }

    private void SetOliviaText(int textID)
    {
        InputManager.Instance.DisableInput();

        dialoguePanel.SetActive(true);

        for (int i = 0; i < oliviaDialogues.Length; i++) {
            oliviaDialogues[i].SetActive(false);
        }
        oliviaDialogues[textID].SetActive(true);

    }

    private void AnimateCircleHoleRadius(float from, float to, float duration)
    {
        uiCircleHoleMat.SetFloat("_Radius", from);

        DOTween.To( () => uiCircleHoleMat.GetFloat("_Radius"),
                        x => uiCircleHoleMat.SetFloat("_Radius", x),
                        to, duration)
                        .SetEase(Ease.OutCubic);
    }

    private void MoveCircleHoleCenter(Vector2 from, Vector2 to, float duration)
    {
        uiCircleHoleMat.SetVector("_Center", from);

        DOTween.To( () => (Vector2)uiCircleHoleMat.GetVector("_Center"),
                        x => uiCircleHoleMat.SetVector("_Center", x),
                        to,duration);
    }

    public void DialogueSkip()
    {
        dialoguePanel.SetActive(false);
        InputManager.Instance.EnableInput();
    }

    public void Match3TilesTutorial()
    {
        _FTUEPanelDefaultContainer.Show();
        SetOliviaText(2);
        MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.525f), 0f);
        AnimateCircleHoleRadius(0.5f, 0.12f, 0.2f);
    }

    public void TileMatchingTutorial()
    {
        if (!GameData.Instance.matched3Tiles)
        {
            _FTUEPanelDefaultContainer.Hide();

            DOVirtual.DelayedCall(0.3f, () =>
            {
                _FTUEPanelDefaultContainer.Show();
                SetOliviaText(3);
                MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0.62f, 0.785f), 0f);
                AnimateCircleHoleRadius(0.5f, 0.04f, 0.2f);

                GameData.Instance.matched3Tiles = true;
                SaveLoadManager.Instance.SaveData();
            });
        }
    }
}
