using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cameraRef;
    [SerializeField] private Transform pivotTransform;

    [SerializeField] private float zoomLerpCoefficient = 0.2f;
    [SerializeField] private float rotationLerpCoefficient = 0.2f;
    [SerializeField] private float translationLerpCoefficient = 0.5f;

    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float rotationStep = 15f;

    [SerializeField] private float zoomMax = 10f;
    [SerializeField] private float zoomMin = 1f;

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
        cameraRef.orthographicSize = Mathf.Lerp(cameraRef.orthographicSize, zoomTarget, zoomLerpCoefficient * Time.deltaTime);
        pivotTransform.transform.rotation = Quaternion.Lerp(pivotTransform.transform.rotation, Quaternion.Euler(0f, rotationTarget, 0f), rotationLerpCoefficient);
        transform.position = Vector3.Lerp(transform.position, positionTarget, translationLerpCoefficient * Time.deltaTime);
    }

    public void ChangeZoom(float zoomAmount)
    {
        zoomTarget = Mathf.Clamp(zoomTarget + zoomAmount, zoomMin, zoomMax);
    }

    public void ChangeRotation(float rotationAmount)
    {
        rotationTarget += rotationAmount;
    }

    public void ChangePosition(Vector3 translationAmount)
    {
        positionTarget += translationAmount;
    }

    [ContextMenu("Zoom In")]
    public void OnZoomIn()
    {
        ChangeZoom(-zoomStep);
    }

    [ContextMenu("Zoom Out")]
    public void OnZoomOut()
    {
        ChangeZoom(zoomStep);
    }

    [ContextMenu("Rotate Left")]
    public void OnRotateLeft()
    {
        ChangeRotation(-rotationStep);
    }

    [ContextMenu("Rotate Right")]
    public void OnRotateRight()
    {
        ChangeRotation(rotationStep);
    }
}