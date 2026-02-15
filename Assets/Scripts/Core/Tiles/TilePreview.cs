using UnityEngine;

[RequireComponent(typeof(TileCalculator))]
public class TilePreview : MonoBehaviour
{
    [SerializeField] private float tileElevation = 0f;
    [SerializeField] private TileCalculator tileCalculator;
    [SerializeField] private TilePreviewInput tilePreviewInput;

    [Header("Feedbacks")]
    [SerializeField] private TileDenyingPreviewFeedback tileDenyingPreviewFeedback;
    [SerializeField] private TilePlacementFeedback tilePlacementFeedback;

    private float targetRotationContinuous;
    private float targetRotationDiscrete;

    private Tile tileInstance;
    private Tile tileInstanceUI;
    private Hexagons.Coords tileInstanceCoords;

    public Tile PreviewTile => tileInstance;

    void OnEnable()
    {
        tilePreviewInput.OnRotation += RotatePreviewTile;
    }

    void OnDisable()
    {
        tilePreviewInput.OnRotation -= RotatePreviewTile;
    }

    public void StartPreviewAtCoords(Tile tile, Tile tileUI, Hexagons.Coords coords)
    {
        tileInstance = tile;
        tileInstanceUI = tileUI;
        tileInstanceCoords = coords;

        tilePlacementFeedback.Activate(tileInstance.transform);

        AudioManager.Instance.PlaySound("Tile", "Tile Choosing");

        targetRotationContinuous = tile.RotationOffsetDiscrete;
        targetRotationDiscrete = tile.RotationOffsetDiscrete;
        
        transform.position = Hexagons.HexToWorld(coords);

        tileInstance.transform.SetParent(transform, false);
        tileInstance.transform.localPosition = new Vector3(0f, tileElevation, 0f);
        
        tilePreviewInput.UnlockRotation();
        tileCalculator.CalculateBonuses(tileInstanceCoords, tileInstance);
    }

    private void RotatePreviewTile(float rotationAmount)
    {
        if (tileInstance != null)
        {
            targetRotationContinuous += rotationAmount;
            float newRotationDiscrete = Mathf.Floor(targetRotationContinuous / 60f) * 60f;

            if (newRotationDiscrete != targetRotationDiscrete)
            {
                AudioManager.Instance.PlaySound("Tile", "Tile Rotating");

                targetRotationDiscrete = newRotationDiscrete;
                tileInstance.Rotate(targetRotationDiscrete);
                tileInstanceUI.Rotate(targetRotationDiscrete);
                tileCalculator.CalculateBonuses(tileInstanceCoords, tileInstance);
            }
        }
    }

    public void EndPreview(bool isTileAccepted)
    {
        tilePreviewInput.LockRotation();
        tilePlacementFeedback.StopHovering();

        if (!isTileAccepted)
        {
            tileCalculator.DestroyBonsuses();
            tileDenyingPreviewFeedback.Activate(tileInstance.gameObject);
        }
        else {
            tileCalculator.ProcessBonsuses();
        }
        
        StarCollecting.Instance.SetStarScoreMainMode();
        tileInstance = null;
    }
}
