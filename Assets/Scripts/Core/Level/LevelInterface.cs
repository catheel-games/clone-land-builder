using System;
using UnityEngine;

public class LevelInterface : MonoBehaviour
{
    [SerializeField] private TilePreviewControlContainer tilePreviewControlContainer;
    [SerializeField] private CameraControlContainer cameraControlContainer;

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
    }

    private void CameraControlClick(CameraControlContainer.ControlButton controlButton)
    {
        OnCameraControlClick?.Invoke(controlButton);
    }
}
