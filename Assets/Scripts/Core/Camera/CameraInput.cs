using System;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private float zoomCoefficient = 2f;
    [SerializeField] private float rotationCoefficient = 0.5f;
    [SerializeField] private float translationCoefficient = 4f;

    private bool isTranslatable = true;

    public void LockPosition() => isTranslatable = false;
    public void UnlockPosition() => isTranslatable = true;

    public event Action<Vector3> OnTranslation;
    public event Action<float> OnRotation;
    public event Action<float> OnZoom;

    void OnEnable()
    {
        InputManager.OnOneFingerSlide += OnOneFingerSlide;
        InputManager.OnTwoFingerSlide += OnTwoFingerSlide;
        InputManager.OnMouseLeftClickSlide += OnMouseLeftClickSlide;
        InputManager.OnMouseRightClickSlide += OnMouseRightClickSlide;
    }

    void OnDisable()
    {
        InputManager.OnOneFingerSlide -= OnOneFingerSlide;
        InputManager.OnTwoFingerSlide -= OnTwoFingerSlide;
        InputManager.OnMouseLeftClickSlide -= OnMouseLeftClickSlide;
        InputManager.OnMouseRightClickSlide -= OnMouseRightClickSlide;
    }

    private void OnOneFingerSlide(InputManager.OneFingerSlideEvent slideEvent)
    {
        if (isTranslatable)
        {
            Vector2 touchPositionA = slideEvent.Position - slideEvent.Delta;
            Vector2 touchPositionB = slideEvent.Position;

            Vector3 touchPositionA3 = new Vector3(touchPositionA.x, 0f, touchPositionA.y);
            Vector3 touchPositionB3 = new Vector3(touchPositionB.x, 0f, touchPositionB.y);

            OnTranslation?.Invoke((touchPositionB3 - touchPositionA3) * translationCoefficient);
        }
    }

    private void OnTwoFingerSlide(InputManager.TwoFingerSlideEvent slideEvent)
    {
        Vector2 firstDistanceNormal = (slideEvent.SecondFinger.Position - slideEvent.FirstFinger.Position).normalized;
        Vector2 firstDistanceTangent = new Vector2(-firstDistanceNormal.y, firstDistanceNormal.x);

        float firstDeltaAlongNormal = Vector2.Dot(slideEvent.FirstFinger.Delta, firstDistanceNormal);
        float firstDeltaAlongTangent = Vector2.Dot(slideEvent.FirstFinger.Delta, firstDistanceTangent);   
        
        Vector2 secondDistanceNormal = -firstDistanceNormal;
        Vector2 secondDistanceTangent = -firstDistanceTangent;

        float secondDeltaAlongNormal = Vector2.Dot(slideEvent.SecondFinger.Delta, secondDistanceNormal);
        float secondDeltaAlongTangent = Vector2.Dot(slideEvent.SecondFinger.Delta, secondDistanceTangent);

        OnRotation?.Invoke((firstDeltaAlongTangent + secondDeltaAlongTangent) * rotationCoefficient);
        OnZoom?.Invoke((firstDeltaAlongNormal + secondDeltaAlongNormal) * zoomCoefficient);
    }

    private void OnMouseLeftClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        if (isTranslatable)
        {
            Vector2 mousePositionA = clickEvent.Position - clickEvent.Delta;
            Vector2 mousePositionB = clickEvent.Position;

            Vector3 mousePositionA3 = new Vector3(mousePositionA.x, 0f, mousePositionA.y);
            Vector3 mousePositionB3 = new Vector3(mousePositionB.x, 0f, mousePositionB.y);

            OnTranslation?.Invoke((mousePositionB3 - mousePositionA3) * translationCoefficient);
        }
    }

    private void OnMouseRightClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        Vector2 firstDistanceNormal = (-clickEvent.Position).normalized;
        Vector2 firstDistanceTangent = new Vector2(-firstDistanceNormal.y, firstDistanceNormal.x);

        float firstDeltaAlongNormal = Vector2.Dot(clickEvent.Delta, firstDistanceNormal);
        float firstDeltaAlongTangent = Vector2.Dot(clickEvent.Delta, firstDistanceTangent);   

        OnRotation?.Invoke(firstDeltaAlongTangent * rotationCoefficient);
        OnZoom?.Invoke(firstDeltaAlongNormal * zoomCoefficient);
    }
}
