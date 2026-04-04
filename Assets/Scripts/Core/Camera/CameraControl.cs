using DG.Tweening;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera previewCamera;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private CameraInput cameraInput;
    [SerializeField] private TileGrid tileGrid;

    [Header("Lerp Coefficients")]
    [SerializeField] private float zoomLerpCoefficient = 10f;
    [SerializeField] private float rotationLerpCoefficient = 10f;
    [SerializeField] private float translationLerpCoefficient = 10f;

    [Header("Boundary Coefficients")]
    [SerializeField] private float zoomRangeCoefficient = 2f;
    [SerializeField] private float translationRangeCoefficient = 8f;
    
    [Header("Discrete Steps")]
    [SerializeField] private float zoomStep = 2f;
    [SerializeField] private float rotationStep = 20f;

    [Header("Presentation Properties")]
    [SerializeField] private float presentationZoomCoefficient = 6f;
    [SerializeField] private float presentationRotationStep = -10f;
    [SerializeField] private float presentationYPos = 7f;
    [SerializeField] private Vector3 presentationRotation;

    private bool staticRotation = false;

    private float zoomTarget;
    private float rotationTarget;
    private Vector3 positionTarget;

    public float PivotRotation => pivotTransform.rotation.eulerAngles.y;

    void OnEnable()
    {
        cameraInput.OnTranslation += ChangePosition;
        cameraInput.OnRotation += ChangeRotation;
        cameraInput.OnZoom += ChangeZoom;
    }

    void OnDisable()
    {
        cameraInput.OnTranslation -= ChangePosition;
        cameraInput.OnRotation -= ChangeRotation;
        cameraInput.OnZoom -= ChangeZoom;
    }

    void Awake()
    {
        zoomTarget = 6 * zoomRangeCoefficient;
        
        rotationTarget = pivotTransform.transform.rotation.y;
        positionTarget = transform.position;
    }

    void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            zoomTarget,
            zoomLerpCoefficient * Time.deltaTime
        );

        previewCamera.orthographicSize = Mathf.Lerp(
            previewCamera.orthographicSize,
            zoomTarget,
            zoomLerpCoefficient * Time.deltaTime
        );

        if (staticRotation) {
            rotationTarget += presentationRotationStep * Time.deltaTime;
        }

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

    // Mechanical
    public void LockPosition(Vector3 newPosition)
    {
        cameraInput.LockPosition();
        positionTarget = newPosition;
    }

    public void UnlockPosition()
    {
        cameraInput.UnlockPosition();
    }

    // Physical
    private void ChangeZoom(float zoomAmount)
    {
        float zoomMin = zoomRangeCoefficient * 5f;
        float zoomMax = zoomRangeCoefficient * (6f + tileGrid.TileGridDiameter);
        
        zoomTarget = Mathf.Clamp(zoomTarget + zoomAmount, zoomMin, zoomMax);
    }

    private void ChangeRotation(float rotationAmount)
    {
        rotationTarget -= rotationAmount;
    }

    private void ChangePosition(Vector3 translationAmount)
    {
        positionTarget -= pivotTransform.transform.rotation * translationAmount * zoomTarget;

        float minX = tileGrid.TileGridCenter.x - tileGrid.TileGridDiameter - translationRangeCoefficient;
        float maxX = tileGrid.TileGridCenter.x + tileGrid.TileGridDiameter + translationRangeCoefficient;
        float minZ = tileGrid.TileGridCenter.z - tileGrid.TileGridDiameter - translationRangeCoefficient;
        float maxZ = tileGrid.TileGridCenter.z + tileGrid.TileGridDiameter + translationRangeCoefficient;

        positionTarget = new Vector3(
            Mathf.Clamp(positionTarget.x, minX, maxX),
            positionTarget.y,
            Mathf.Clamp(positionTarget.z, minZ, maxZ)
        );
    }

    public void RotateLeft()
    {
        ChangeRotation(rotationStep * Time.deltaTime);
    }

    public void RotateRight()
    {
        ChangeRotation(-rotationStep * Time.deltaTime);
    }

    public void ZoomIn()
    {
        ChangeZoom(-zoomStep * Time.deltaTime);
    }

    public void ZoomOut()
    {
        ChangeZoom(zoomStep * Time.deltaTime);
    }

    public void CityPresentation()
    {
        Vector3 presentationPosition = tileGrid.TileGridCenter;
        
        mainCamera.transform.DOMoveY(presentationYPos, 0.5f);
        mainCamera.transform.DOLocalRotate(presentationRotation, 0.5f);

        zoomTarget = zoomRangeCoefficient * (8f + tileGrid.TileGridDiameter + presentationZoomCoefficient);
        LockPosition(presentationPosition);
        
        staticRotation = true;
        cameraInput.enabled = false;
    }
}