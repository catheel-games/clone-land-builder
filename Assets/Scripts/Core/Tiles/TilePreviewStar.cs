using UnityEngine;

public class TilePreviewStar : MonoBehaviour
{
    [SerializeField] private float rotationCoefficient = 40f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationCoefficient * Time.deltaTime);
    }
}
