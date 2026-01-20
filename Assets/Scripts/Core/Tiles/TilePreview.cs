using UnityEngine;

[RequireComponent(typeof(TileCalculator))]
public class TilePreview : MonoBehaviour
{
    [SerializeField] private float tileElevevation = 0.5f;
    [SerializeField] private TileCalculator tileCalculator;

    private Tile tileInstance;
    private TileGrid tileGrid;

    private float targetRotationContinuous;
    private float targetRotationDiscrete;
    private float newRotationDiscrete;

    public void Init(TileGrid tileGrid, Hexagons.Coords coords)
    {
        this.tileGrid = tileGrid;
        
        SetupTile();
        tileCalculator.Init(tileInstance, tileGrid, coords);
        tileCalculator.Process();
    }

    private void SetupTile()
    {
        tileInstance = TileGenerator.Instance.GetRandomTile();
        tileInstance.transform.SetParent(transform, false);
        tileInstance.transform.localPosition = new Vector3(0f, tileElevevation, 0f);
    }

    public Tile PlaceTile()
    {
        tileInstance.Place();
        tileInstance.transform.SetParent(tileGrid.transform, true);
        
        return tileInstance;
    }

    public void RotateContinuously(float rotationAmount)
    {
        targetRotationContinuous += rotationAmount;
        newRotationDiscrete = Mathf.Floor(targetRotationContinuous / 60f) * 60f;

        if (newRotationDiscrete != targetRotationDiscrete)
        {
            targetRotationDiscrete = newRotationDiscrete;
            RotateDiscretely();
        }
    }

    private void RotateDiscretely()
    {
        tileInstance.Rotate(targetRotationDiscrete);
        tileCalculator.Process();
    }
}
