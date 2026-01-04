using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cameraRef;
    [SerializeField] private Transform pivotTransform;

    [SerializeField] private float zoomLerpCoefficient = 10f;
    [SerializeField] private float rotationLerpCoefficient = 10f;
    [SerializeField] private float translationLerpCoefficient = 10f;

    [SerializeField] private float zoomMin = 1f;
    [SerializeField] private float zoomMax = 10f;

    [SerializeField] private float zoomStep = 2f;
    [SerializeField] private float rotationStep = 20f;

    private float zoomTarget;
    private float rotationTarget;
    private Vector3 positionTarget;

    void Awake()
    {
        zoomTarget = cameraRef.orthographicSize;
        rotationTarget = pivotTransform.transform.rotation.y;
        positionTarget = transform.position;
    }

    void Update()
    {
        cameraRef.orthographicSize = Mathf.Lerp(
            cameraRef.orthographicSize,
            zoomTarget,
            zoomLerpCoefficient * Time.deltaTime
        );

        pivotTransform.transform.rotation = Quaternion.Lerp(
            pivotTransform.transform.rotation,
            Quaternion.Euler(0f, rotationTarget, 0f),
            rotationLerpCoefficient * Time.deltaTime
        );

        transform.position = Vector3.Lerp(
            transform.position,
            positionTarget,
            translationLerpCoefficient * Time.deltaTime
        );
    }

    public void ChangeZoom(float zoomAmount)
    {
        zoomTarget = Mathf.Clamp(zoomTarget + zoomAmount, zoomMin, zoomMax);
    }

    public void ChangeRotation(float rotationAmount)
    {
        rotationTarget -= rotationAmount;
    }

    public void ChangePosition(Vector3 translationAmount)
    {
        positionTarget -= pivotTransform.transform.rotation * translationAmount;
    }

    public void OnZoomIn()
    {
        ChangeZoom(-zoomStep);
    }

    public void OnZoomOut()
    {
        ChangeZoom(zoomStep);
    }

    public void OnRotateLeft()
    {
        ChangeRotation(rotationStep);
    }

    public void OnRotateRight()
    {
        ChangeRotation(-rotationStep);
    }
}