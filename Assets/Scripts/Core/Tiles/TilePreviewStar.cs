using UnityEngine;

public class TilePreviewStar : MonoBehaviour
{
    [SerializeField] private float rotationCoefficient = 40f;
    [SerializeField] private TilePreviewStarScaleInFeedback scaleInFeedback;

    private Hexagons.Coords coords;

    public Hexagons.Coords SideCoords => coords;

    public void Setup(Hexagons.Coords coords)
    {
        this.coords = coords;

        scaleInFeedback.Play();
    }

    public void Kill()
    {
        scaleInFeedback.Kill();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationCoefficient * Time.deltaTime);
    }
}
