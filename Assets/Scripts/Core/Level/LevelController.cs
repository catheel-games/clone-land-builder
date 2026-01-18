using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] private UIContainer tilePlacerContainer;
    [SerializeField] private UIContainer tilePreviewContainer;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private TileGridController tileGridController;

    [SerializeField] private Transform cameraPivotTransform;

    private bool isTileViewMode;
    
    public bool IsTileViewMode => isTileViewMode;
    public float CameraPivotRotation => cameraPivotTransform.rotation.eulerAngles.y;

    void Start()
    {
        isTileViewMode = false;
        tilePlacerContainer.Show();
        tilePreviewContainer.Hide();
    }

    public void EnterTileViewMode(Hexagons.Coords coords)
    {
        isTileViewMode = true;
        tilePlacerContainer.Hide();
        tilePreviewContainer.Show();

        cameraController.SetPosition(Hexagons.HexToWorld(coords));
    }

    public void ExitTileViewMode()
    {
        isTileViewMode = false;
        tilePlacerContainer.Show();
        tilePreviewContainer.Hide();
    }
}
