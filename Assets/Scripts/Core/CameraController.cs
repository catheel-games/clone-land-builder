using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _pivotPoint;

    [SerializeField] private float zoomCoefficient = 0.5f;
    [SerializeField] private float rotateCoefficient = 30f;
    [SerializeField] private float translateCoefficient = 0.5f;

    [SerializeField] private float rotationAmountUI = 15f;
    [SerializeField] private float zoomAmountUI = 0.5f;

    private Vector3 _lastMousePosition;
    private Vector3 _lastRotatePosition;

    private float _zoomTarget;
    private float _rotateTarget;
    private float _currentRotation;

    void Awake()
    {
        _zoomTarget = _camera.orthographicSize;
        _currentRotation = 0f;
        _rotateTarget = 0f;
    }

    void Update()
    {
        HandlePCMovement();
        HandlePCZoom();
        HandlePCRotate();

        // Zoom lerp
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoomTarget, 0.2f);

        // Rotation lerp
        float previousRotation = _currentRotation;
        _currentRotation = Mathf.Lerp(_currentRotation, _rotateTarget, 0.2f);
        float rotationDelta = _currentRotation - previousRotation;

        _camera.transform.RotateAround(_pivotPoint.position, Vector3.up, rotationDelta);
    }

    private void HandlePCMovement()
    {
        if (Input.GetMouseButton(1)) return;

        // Capture mouse cursors position on mouse down
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
        }

        // If mouse is not down, skip the function
        if (!Input.GetMouseButton(0)) return;

        // Get the position of the mouse in world space
        Vector3 mouseWorldPoint = _camera.ScreenToWorldPoint(Input.mousePosition); // current frame
        Vector3 lastMouseWorldPoint = _camera.ScreenToWorldPoint(_lastMousePosition); // last frame

        Vector3 delta = mouseWorldPoint - lastMouseWorldPoint;

        _lastMousePosition = Input.mousePosition;

        _camera.transform.position -= delta; // moves camera to opposite direction 
    }

    private void HandlePCZoom()
    {
        if (Input.GetMouseButton(1)) return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return; // if not zooming skip the rest of the function 

        float newSize = _camera.orthographicSize - scroll * zoomCoefficient;
        _camera.orthographicSize = Mathf.Clamp(newSize, 1f, 10f);

        _zoomTarget = _camera.orthographicSize;
    }

    private void HandlePCRotate()
    {
        if (!Input.GetMouseButton(1)) return;

        float delta = Input.GetAxis("Mouse X");
        float rotation = Mathf.Clamp(delta * rotateCoefficient, -1f, 1f);

        _rotateTarget += rotation; 
    }

    [ContextMenu("Zoom In")]
    public void OnZoomIn()
    {
        _zoomTarget -= zoomAmountUI;
    }

    [ContextMenu("Zoom Out")]
    public void OnZoomOut()
    {
        _zoomTarget += zoomAmountUI;
    }

    [ContextMenu("Rotate Left")]
    public void OnRotateLeft()
    {
        _rotateTarget -= rotationAmountUI;
    }

    [ContextMenu("Rotate Right")]
    public void OnRotateRight()
    {
        _rotateTarget += rotationAmountUI;
    }
}