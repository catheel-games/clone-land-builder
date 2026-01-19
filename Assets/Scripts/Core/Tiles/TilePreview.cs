using UnityEngine;

public class TilePreview : MonoBehaviour
{
    [SerializeField] private float rotationLerpCoefficient = 10f;
    private Tile tileInstance;

    private float targetRotation;

    void Update()
    {
        if (tileInstance == null) return;
        tileInstance.transform.rotation = Quaternion.Lerp(
            tileInstance.transform.rotation,
            Quaternion.Euler(0f, Mathf.Floor(targetRotation / 60f) * 60f, 0f),
            rotationLerpCoefficient * Time.deltaTime
        );
    }

    public void SetTile(Tile tile)
    {
        tileInstance = tile;
        targetRotation = tileInstance.transform.rotation.eulerAngles.y;
    }

    public void ChangeRotation(float rotationAmount)
    {
        targetRotation += rotationAmount;
    }
}
