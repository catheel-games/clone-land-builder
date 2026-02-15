using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelCanvasControl : MonoBehaviour
{
    [SerializeField] private TilePreviewControlContainer tilePreviewControlContainer;
    [SerializeField] private CameraControlContainer cameraControlContainer;
    [SerializeField] private TileQueueInterface tileQueueInterface;

    [SerializeField] private LevelTileUnlocking levelTileUnlocking;

    [SerializeField] private StarCollecting starCollecting;
    [SerializeField] private LevelWinFeedback levelWinFeedback;
    [SerializeField] private LevelLoseFeedback levelLoseFeedback;

    [SerializeField] private DefaultContainer generatorDefaultContainer;
    [SerializeField] private PopupContainer levelCompletedContainer;
    [SerializeField] private PopupContainer coinPanelContainer;
    [SerializeField] private DefaultContainer levelTopPartContainer;
    [SerializeField] private PopupContainer newTileUnlockContainer;
    [SerializeField] private PopupContainer levelFailContainer;
    [SerializeField] private DefaultContainer coinSectionContainer;

    public TileQueueInterface _tileQueueInterface => tileQueueInterface;
    public PopupContainer _levelCompletedContainer => levelCompletedContainer;
    public PopupContainer _coinContainer => coinPanelContainer;
    public PopupContainer _newTileUnlockContainer => newTileUnlockContainer;
    public DefaultContainer _levelTopPartContainer => levelTopPartContainer;
    public PopupContainer _levelFailContainer => levelFailContainer;
    public DefaultContainer _coinSectionContainer => coinSectionContainer;

    public Tile TileQueueFirstTile() => tileQueueInterface.FirstTile();

    public event Action<bool> OnTilePreviewControlClick;
    public event Action<CameraControlContainer.ControlButton> OnCameraControlClick;

    void OnEnable()
    {
        tilePreviewControlContainer.OnControlClick += TilePreviewControlClick;
        cameraControlContainer.OnCameraControlClick += CameraControlClick;
    }

    void OnDisable()
    {
        tilePreviewControlContainer.OnControlClick -= TilePreviewControlClick;
        cameraControlContainer.OnCameraControlClick -= CameraControlClick;
    }

    public void EnterTileViewMode()
    {
        tilePreviewControlContainer.Show();
    }

    private void TilePreviewControlClick(bool isTileAccepted)
    {
        tilePreviewControlContainer.Hide();
        OnTilePreviewControlClick?.Invoke(isTileAccepted);

        if (isTileAccepted)
        {
            starCollecting.SetAcceptFeedback();
        }
    }

    private void CameraControlClick(CameraControlContainer.ControlButton controlButton)
    {
        OnCameraControlClick?.Invoke(controlButton);
    }

    public void TileQueueUpdate(List<Tile> queue)
    {
        tileQueueInterface.UpdateDisplay(queue);
    }

    public void LevelWinning() {
        levelWinFeedback.LevelWinning();
    }

    public void LevelLosing() {
        levelLoseFeedback.LevelLosing();
    }

    public void NewTileUnlocked()
    {
        AudioManager.Instance.PlaySound("Other", "New Tile Unlocked");
        levelTileUnlocking.SetUnlockedObject();
        levelWinFeedback._levelCompletedTextTransform.gameObject.SetActive(false);
        coinPanelContainer.Hide();
        newTileUnlockContainer.Show();
    }

    public void DisableWinContainers()
    {
        tilePreviewControlContainer.Hide();
        cameraControlContainer.Hide();
        generatorDefaultContainer.Hide();
    }

    public void ContinueToNextLevelPanel()
    {
        newTileUnlockContainer.Hide();
        levelCompletedContainer.Show();

        levelWinFeedback.LevelCompletedTextAnimation();
    }
}
