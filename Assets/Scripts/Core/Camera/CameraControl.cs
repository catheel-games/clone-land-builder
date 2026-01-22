using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera controlledCamera;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private CameraInput cameraInput;

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
        zoomTarget = controlledCamera.orthographicSize;
        rotationTarget = pivotTransform.transform.rotation.y;
        positionTarget = transform.position;
    }

    void Update()
    {
        controlledCamera.orthographicSize = Mathf.Lerp(
            controlledCamera.orthographicSize,
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
        zoomTarget = Mathf.Clamp(zoomTarget + zoomAmount, zoomMin, zoomMax);
    }

    private void ChangeRotation(float rotationAmount)
    {
        rotationTarget -= rotationAmount;
    }

    private void ChangePosition(Vector3 translationAmount)
    {
        positionTarget -= pivotTransform.transform.rotation * translationAmount;
    }

    public void RotateLeft()
    {
        ChangeRotation(rotationStep);
    }

    public void RotateRight()
    {
        ChangeRotation(-rotationStep);
    }

    public void ZoomIn()
    {
        ChangeZoom(-zoomStep);
    }

    public void ZoomOut()
    {
        ChangeZoom(zoomStep);
    }
}