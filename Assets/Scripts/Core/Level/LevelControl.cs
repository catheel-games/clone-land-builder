using UnityEngine;

public class LevelControl : Singleton<LevelControl>
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private TileControl tileControl;

    public float CameraPivotRotation => cameraControl.PivotRotation;

    void OnEnable()
    {
        tileControl.OnTilePlacerClick += EnterTileViewMode;
        tileControl.OnTileGeneratorUpdate += levelCanvasControl.TileQueueUpdate;
        tileControl.OnTileQueueFirstTileRequest += levelCanvasControl.TileQueueFirstTile;
        levelCanvasControl.OnTilePreviewControlClick += ExitTileViewMode;
        levelCanvasControl.OnCameraControlClick += CameraControlClick;
    }

    void OnDisable()
    {
        tileControl.OnTilePlacerClick -= EnterTileViewMode;
        tileControl.OnTileGeneratorUpdate -= levelCanvasControl.TileQueueUpdate;
        tileControl.OnTileQueueFirstTileRequest -= levelCanvasControl.TileQueueFirstTile;
        levelCanvasControl.OnTilePreviewControlClick -= ExitTileViewMode;
        levelCanvasControl.OnCameraControlClick -= CameraControlClick;
    }

    private void EnterTileViewMode(Hexagons.Coords coords)
    {
        levelCanvasControl.EnterTileViewMode();
        cameraControl.LockPosition(Hexagons.HexToWorld(coords));
    }

    private void ExitTileViewMode(bool isTileAccepted)
    {
        cameraControl.UnlockPosition();
        tileControl.ExitTilePreview(isTileAccepted);
    }

    private void CameraControlClick(CameraControlContainer.ControlButton controlButton)
    {
        switch(controlButton)
        {
            case CameraControlContainer.ControlButton.RotateLeft:
                cameraControl.RotateLeft();
                break;
            case CameraControlContainer.ControlButton.RotateRight:
                cameraControl.RotateRight();
                break;
            case CameraControlContainer.ControlButton.ZoomIn:
                cameraControl.ZoomIn();
                break;
            case CameraControlContainer.ControlButton.ZoomOut:
                cameraControl.ZoomOut();
                break;
        }
    }
}
