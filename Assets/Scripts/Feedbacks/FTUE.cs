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


    private float _screenModifier;
    private bool eiffelUnlocked = false;
    private bool blueSphereTutor = false;

    private void Start()
    {
        uiCircleHoleMat = Instantiate(dialoguePanelImage.material);
        dialoguePanelImage.material = uiCircleHoleMat;
    }

    public void Setup()
    {
        fTUEisOn = true;

        _FTUEPanelDefaultContainer.Show();
        cameraControlContainer.Hide();
        handRectTransform.localPosition = tileSetPos;

        cameraControl.LockPosition(tilePositions[0]);
        InputManager.Instance.DisableInput();
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
            }
            else if (LevelControl.Instance.fTUEtileID == 1)
            {
                InputManager.Instance.EnableInput();

                declineButton.enabled = true;
                declineButtonImage.sprite = declineDefaultSprite;
                handRectTransform.localPosition = rotatePos;

                handAnimator.SetTrigger("Rotate");
            }
            else if (LevelControl.Instance.fTUEtileID == 2)
            {
                InputManager.Instance.EnableInput();

                _FTUEPanelDefaultContainer.Show();
                SetOliviaText(0);
                MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0, -50), 0f, new Vector2(0.5f, 0.5f));
                AnimateCircleHoleRadius(0.5f, 0.08f, 0.2f);
            }
        }
    }

    public void TileAccept(bool isTileAccepted)
    {
        if (fTUEisOn)
        {
            InputManager.Instance.DisableInput();
            DOVirtual.DelayedCall(0.02f, () => cameraControlContainer.Hide());

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
                    });
                }
            }

            else if (isTileAccepted)
            {
                _FTUEPanelDefaultContainer.Hide();

                LevelControl.Instance.fTUEtileID++;

                if (LevelControl.Instance.fTUEtileID == 1)
                {
                    //InputManager.Instance.EnableInput();

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
                    DOVirtual.DelayedCall(0.03f, () => cameraControlContainer.Show());

                    SetOliviaText(1);
                    MoveCircleHoleCenter(new Vector2(0.5f, 0.475f), new Vector2(125f, -350f), 0f, new Vector2(0, 1));
                    AnimateCircleHoleRadius(0.5f, 0.045f, 0.2f);

                    SaveLoadManager.Instance.gameData.ftueIsEnded = true;
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
                cameraControlContainer.Show();
            }
        }
    }

    private void SetOliviaText(int textID)
    {
        InputManager.Instance.DisableInput();
        AudioManager.Instance.PlaySound("UI", "Tutorial Olivia");

        dialoguePanel.SetActive(true);

        for (int i = 0; i < oliviaDialogues.Length; i++) {
            oliviaDialogues[i].SetActive(false);
        }
        oliviaDialogues[textID].SetActive(true);

    }

    private void AnimateCircleHoleRadius(float from, float to, float duration)
    {
        uiCircleHoleMat.SetFloat("_Radius", from);

        DOTween.To(() => uiCircleHoleMat.GetFloat("_Radius"),
                        x => uiCircleHoleMat.SetFloat("_Radius", x),
                        to, duration)
                        .SetEase(Ease.OutCubic);
    }

    private void MoveCircleHoleCenter(Vector2 from, Vector2 newPos, float duration, Vector2 anchor)
    {
        _screenModifier = 1;

        if (Screen.width < 1080) {
            _screenModifier = 1080f / Screen.width;
            Debug.Log(_screenModifier);
        }


        Vector4 screenVector = new Vector4(Screen.width * _screenModifier, Screen.height * _screenModifier, 0, 0);

        uiCircleHoleMat.SetVector("_RectSize", screenVector);

        uiCircleHoleMat.SetVector("_Center", from);

        Vector2 to = new Vector2((newPos.x / screenVector.x) + anchor.x, (newPos.y / screenVector.y) + anchor.y);

        DOTween.To(() => (Vector2)uiCircleHoleMat.GetVector("_Center"),
                        x => uiCircleHoleMat.SetVector("_Center", x),
                        to, duration);
    }

    public void DialogueSkip()
    {
        if (blueSphereTutor)
        {
            blueSphereTutor = false;

            cameraControl.UnlockPosition();
            uiCircleHoleMat.SetVector("_Color", new Vector4(0, 0, 0, 0.7f));
            dialoguePanel.SetActive(false);
            InputManager.Instance.EnableInput();
        }
        else if (!eiffelUnlocked)
        {
            uiCircleHoleMat.SetVector("_Color", new Vector4(0, 0, 0, 0.7f));
            dialoguePanel.SetActive(false);
            InputManager.Instance.EnableInput();
        }
        else
        {
            uiCircleHoleMat.SetVector("_Color", new Vector4(0, 0, 0, 0));
            eiffelUnlocked = false;
            SetOliviaText(5);
        }
    }

    public void Match3TilesTutorial()
    {
        _FTUEPanelDefaultContainer.Show();
        handAnimator.gameObject.SetActive(false);
        SetOliviaText(2);
        MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0, 50), 0f, new Vector2(0.5f, 0.5f));
        AnimateCircleHoleRadius(0.5f, 0.12f, 0.2f);
    }

    public void TileMatchingTutorial()
    {
        if (!SaveLoadManager.Instance.gameData.matched3Tiles)
        {
            _FTUEPanelDefaultContainer.Hide();

            DOVirtual.DelayedCall(0.3f, () =>
            {
                _FTUEPanelDefaultContainer.Show();
                handAnimator.gameObject.SetActive(false);
                SetOliviaText(3);
                MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(-410, -390), 0f, new Vector2(1, 1));
                AnimateCircleHoleRadius(0.5f, 0.04f, 0.2f);

                SaveLoadManager.Instance.gameData.matched3Tiles = true;
                SaveLoadManager.Instance.SaveData();
            });
        }
    }

    public void EiffelUnlockingTutorial()
    {
        eiffelUnlocked = true;

        handAnimator.gameObject.SetActive(false);
        _FTUEPanelDefaultContainer.Show();
        SetOliviaText(4);
        MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(55, -335), 0f, new Vector2(0.5f, 1f));
    }

    public void BlueSphereTutorial()
    {
        blueSphereTutor = true;

        handAnimator.gameObject.SetActive(false);
        _FTUEPanelDefaultContainer.Show();
        SetOliviaText(6);
        AnimateCircleHoleRadius(0.5f, 0.07f, 0.2f);
        MoveCircleHoleCenter(new Vector2(0.5f, 0.5f), new Vector2(0, 0), 0f, new Vector2(0.5f, 0.5f));
    }
}
