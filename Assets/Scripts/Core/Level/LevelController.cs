using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] private UIContainer tilePlacerContainer;
    [SerializeField] private UIContainer tilePreviewContainer;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private TileGrid tileGridController;

    [SerializeField] private Transform cameraPivotTransform;

    [SerializeField] private float tileRotationLerpCoefficient = 10f;

    private bool isTileViewMode;
    
    public bool IsTileViewMode => isTileViewMode;
    public float CameraPivotRotation => cameraPivotTransform.rotation.eulerAngles.y;
    public float TileRotationLerpCoefficient => tileRotationLerpCoefficient;

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
