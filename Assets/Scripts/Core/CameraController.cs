using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector3 _lastMousePosition;
    private Vector3 _lastRotatePosition;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _pivotPoint;
    [SerializeField] private Button ZoomIn = null;
    [SerializeField] private Button ZoomOut = null;
    [SerializeField] private Button RotateLeft = null;
    [SerializeField] private Button RotateRight = null;

    void Start()
    {
        ZoomIn.onClick.AddListener(OnZoomIn);
        ZoomOut.onClick.AddListener(OnZoomOut);
        RotateLeft.onClick.AddListener(OnRotateLeft);
        RotateRight.onClick.AddListener(OnRotateRight);
    }

    void Update()
    {
        // CheckClick();
        HandlePCMovement();
        HandlePCZoom();
        HandlePCRotate();
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

        float zoomSpeed = 0.5f;
        float newSize = _camera.orthographicSize - scroll * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(newSize, 1f, 10f);
    }

    private void HandlePCRotate()
    {
        if (!Input.GetMouseButton(1)) return;

        float delta = Input.GetAxis("Mouse X");
        float rotateSpeed = 30f;
        float rotation = Mathf.Clamp(delta * rotateSpeed, -1f, 1f);

        _camera.transform.RotateAround(_pivotPoint.position, Vector3.up, -rotation);
    }

    public void OnZoomIn()
    {
        float zoomSpeed = 0.5f;
        float newSize = _camera.orthographicSize - zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(newSize, 1f, 10f);
    }

    public void OnZoomOut()
    {
        float zoomSpeed = 0.5f;
        float newSize = _camera.orthographicSize + zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(newSize, 1f, 10f);
    }

    public void OnRotateLeft()
    {
        float rotateAmount = 15f;
        _camera.transform.RotateAround(_pivotPoint.position, Vector3.up, rotateAmount);
    }

    public void OnRotateRight()
    {
        float rotateAmount = 15f;
        _camera.transform.RotateAround(_pivotPoint.position, Vector3.up, -rotateAmount);
    }
}