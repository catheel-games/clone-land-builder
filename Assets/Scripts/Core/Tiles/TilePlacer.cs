using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private RectTransform tilePlacerPlus;

    private Transform tempCameraPivot;
    private Hexagons.Coords coords;

    void Start()
    {
        tempCameraPivot = Camera.main.transform.parent;
    }

    void LateUpdate()
    {
        tilePlacerPlus.localRotation = Quaternion.Euler(0f, 0f, -tempCameraPivot.rotation.eulerAngles.y);
    }

    public void SetCoords(Hexagons.Coords coords)
    {
        this.coords = coords;
    }

    public void Click()
    {
        // TileGrid.Instance.SetTile(coords);
        TilePreview.Instance.Preview(coords);
    }
}
