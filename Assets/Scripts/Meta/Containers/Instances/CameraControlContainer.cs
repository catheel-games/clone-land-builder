using System;

public class CameraControlContainer : UIContainer
{
    public enum ControlButton
    {
        RotateLeft,
        RotateRight,
        ZoomIn,
        ZoomOut    
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
