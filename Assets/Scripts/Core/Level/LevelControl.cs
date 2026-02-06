using UnityEngine;
using System.Collections.Generic;

public class LevelControl : Singleton<LevelControl>
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private TileControl tileControl;

    [SerializeField] private StarCollecting starCollecting;
    [SerializeField] private LevelDataSetup levelDataSetup;
    [SerializeField] private FTUE _FTUE;

    public int fTUEtileID = 0;
    public int starScore;
    public int tileScore = 50;

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

    private void Start()
    {
        AudioManager.Instance.PlaySound("Other", "Level Start");
    }

    private void EnterTileViewMode(Hexagons.Coords coords)
    {
        levelCanvasControl.EnterTileViewMode();
        cameraControl.LockPosition(Hexagons.HexToWorld(coords));

        _FTUE.SetTile();
    }

    private void ExitTileViewMode(bool isTileAccepted)
    {
        _FTUE.TileAccept(isTileAccepted);

        cameraControl.UnlockPosition();
        tileControl.ExitTilePreview(isTileAccepted);

    }

    private void CameraControlClick(CameraControlContainer.ControlButton controlButton)
    {
        switch (controlButton)
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

    public void SetPlusObjects(int starAmount, List<GameObject> starObjects, GameObject tileObject)
    {
        starCollecting.GetPlusObjects(starAmount, starObjects, tileObject);

        if (tileObject != null)
            _FTUE.TileMatchingTutorial();
    }

    public void RotateTileFTUE()
    {
        _FTUE.TileRotate();
    }

    public void Match3Tiles()
    {
        _FTUE.Match3TilesTutorial();
    }

    public void SaveTile()
    {
        levelDataSetup.SaveData();
    }

}
