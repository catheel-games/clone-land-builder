using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TilePreview : Singleton<TilePreview>
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Button declineButton;
    [SerializeField] private Button acceptButton;

    private GameObject previewTile;
    private bool isPreviewTileActive;
    private RaycastHit hit;

    private Hexagons.Coords coords;

    void Start()
    {
        declineButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Debug.Log("enable working");
        InputManager.OnOneFingerSlide += OnFingerDrag;
        InputManager.OnMouseRightClickSlide += OnMouseDrag;
    }

    void OnDisable()
    {
        InputManager.OnOneFingerSlide -= OnFingerDrag;
        InputManager.OnMouseRightClickSlide -= OnMouseDrag;
    }

    private void OnMouseDrag(InputManager.MouseClickEvent click)
    {
        Debug.Log("dzec ashxatec");
        Ray ray = Camera.main.ScreenPointToRay(click.Position);

        if (Physics.Raycast(ray, out hit))
        {
            if (previewTile != null && hit.collider.transform.IsChildOf(previewTile.transform))
            {
                RotateTile(click.Delta.x);
            }
        }
    }

    private void OnFingerDrag(InputManager.OneFingerSlideEvent slide)
    {
        Ray ray = Camera.main.ScreenPointToRay(slide.Position);

        if (Physics.Raycast(ray, out hit))
        {
            if (previewTile != null && hit.collider.transform.IsChildOf(previewTile.transform))
            {
                RotateTile(slide.Delta.x);
            }
        }
    }

    private void RotateTile(float drag)
    {
        if (isPreviewTileActive)
        {
            FindObjectOfType<CameraInputInterpreter>().blockInput = true;
            if (drag < 0) previewTile.GetComponent<Tile>().Rotate(-1);
            if (drag > 0) previewTile.GetComponent<Tile>().Rotate(1);
        }
    }

    public void Preview(Hexagons.Coords coords)
    {
        isPreviewTileActive = true;
        TileGrid.Instance.hideTilePlacer();

        Vector3 worldPosition = Hexagons.HexToWorld(coords);
        worldPosition.y += 0.5f;

        previewTile = Instantiate(
                    tilePrefab,
                    worldPosition,
                    Quaternion.identity,
                    transform
        );

        this.coords = coords;
        
        declineButton.gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(true);
    }

    public void DeclineTile()
    {
        Destroy(previewTile);
        TileGrid.Instance.showTilePlacer();

        isPreviewTileActive = false;
        FindObjectOfType<CameraInputInterpreter>().blockInput = false;
        
        declineButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
    }

    public void AcceptTile()
    {
        Destroy(previewTile);
        TileGrid.Instance.SetTile(coords);
        TileGrid.Instance.showTilePlacer();

        isPreviewTileActive = false;
        FindObjectOfType<CameraInputInterpreter>().blockInput = false;
        
        declineButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
    }
}
