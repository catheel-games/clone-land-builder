using UnityEngine;

public class TilePreview : MonoBehaviour
{
    [SerializeField] private float rotationLerpCoefficient = 10f;
    [SerializeField] private Tile tileInstance;

    private float targetRotation;

    void Awake()
    {
        targetRotation = tileInstance.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        tileInstance.transform.rotation = Quaternion.Lerp(
            tileInstance.transform.rotation,
            Quaternion.Euler(0f, Mathf.Floor(targetRotation / 60f) * 60f, 0f),
            rotationLerpCoefficient * Time.deltaTime
        );
    }

    public void ChangeRotation(float rotationAmount)
    {
        targetRotation += rotationAmount;
    }
}
