using System;
using UnityEngine;

public class CameraControlContainer : UIContainer
{
    public enum ControlButton
    {
        RotateLeft,
        RotateRight,
        ZoomIn,
        ZoomOut    
    }

    [Header("Buttons")]
    [SerializeField] private UIButton rotateLeftButton;
    [SerializeField] private UIButton rotateRightButton;
    [SerializeField] private UIButton zoomInButton;
    [SerializeField] private UIButton zoomOutButton;

    void OnEnable()
    {
        rotateLeftButton.OnHold += RotateLeft;
        rotateRightButton.OnHold += RotateRight;
        zoomInButton.OnHold += ZoomIn;
        zoomOutButton.OnHold += ZoomOut;
    }

    void OnDisable()
    {
        rotateLeftButton.OnHold -= RotateLeft;
        rotateRightButton.OnHold -= RotateRight;
        zoomInButton.OnHold -= ZoomIn;
        zoomOutButton.OnHold -= ZoomOut;
    }

    public event Action<ControlButton> OnCameraControlClick;
    
    public void RotateLeft()
    {
        OnCameraControlClick?.Invoke(ControlButton.RotateLeft);
    }

    public void RotateRight()
    {
        OnCameraControlClick?.Invoke(ControlButton.RotateRight);
    }

    public void ZoomIn()
    {
        OnCameraControlClick?.Invoke(ControlButton.ZoomIn);
    }

    public void ZoomOut()
    {
        OnCameraControlClick?.Invoke(ControlButton.ZoomOut);
    }
}
