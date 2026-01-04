using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _pivotPoint;

    [SerializeField] private float zoomLerpCoefficient = 0.2f;
    [SerializeField] private float rotationLerpCoefficient = 0.2f;
    [SerializeField] private float translationLerpCoefficient = 0.5f;

    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float rotationStep = 15f;

    [SerializeField] private float zoomMax = 10f;
    [SerializeField] private float zoomMin = 1f;

    private float _zoomTarget;
    private float _rotationTarget;
    private Vector3 _positionTarget;

    private float _currentRotation;

    void Awake()
    {
        _zoomTarget = _camera.orthographicSize;
        _currentRotation = 0f;
        _rotationTarget = 0f;
        _positionTarget = _camera.transform.position;
    }

    void Update()
    {
        // Zoom lerp
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoomTarget, zoomLerpCoefficient * Time.deltaTime);

        // Rotation lerp
        float rotationDelta = Mathf.Lerp(_currentRotation, _rotationTarget, rotationLerpCoefficient * Time.deltaTime) - _currentRotation;
        _camera.transform.RotateAround(_pivotPoint.position, Vector3.up, rotationDelta);

        // Translation Lerp
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _positionTarget, translationLerpCoefficient * Time.deltaTime);
    }

    public void ChangeZoom(float zoomAmount)
    {
        _zoomTarget = Mathf.Clamp(_zoomTarget + zoomAmount, zoomMin, zoomMax);
    }

    public void ChangeRotation(float rotationAmount)
    {
        _rotationTarget += rotationAmount;
    }

    public void ChangePosition(Vector3 translationAmount)
    {
        _positionTarget += translationAmount;
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