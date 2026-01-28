using UnityEngine;

public class TilePreviewStar : MonoBehaviour
{
    [SerializeField] private float rotationCoefficient = 40f;
    [SerializeField] private TilePreviewStarScaleInFeedback scaleInFeedback;

    void Start()
    {
        scaleInFeedback.Play();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationCoefficient * Time.deltaTime);
    }
}
