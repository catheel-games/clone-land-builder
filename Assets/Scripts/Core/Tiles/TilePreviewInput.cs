using System;
using UnityEngine;

public class TilePreviewInput : MonoBehaviour
{ 
    [SerializeField] private float rotationCoefficient = 0.5f;
    private TilePreview tilePreviewInstance;

    private bool isRotatable = false;

    public void LockRotation() => isRotatable = false;
    public void UnlockRotation() => isRotatable = true;

    public event Action<float> OnRotation;

    void OnEnable()
    {
        InputManager.OnOneFingerSlide += OnOneFingerSlide;
        InputManager.OnMouseLeftClickSlide += OnMouseLeftClickSlide;
    }

    void OnDisable()
    {
        InputManager.OnOneFingerSlide -= OnOneFingerSlide;
        InputManager.OnMouseLeftClickSlide -= OnMouseLeftClickSlide;
    }

    private void OnOneFingerSlide(InputManager.OneFingerSlideEvent slideEvent)
    {
        if (isRotatable)
        {
            Vector2 distanceNormal = -slideEvent.Position.normalized;
            Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

            float deltaAlongTangent = Vector2.Dot(slideEvent.Delta, distanceTangent);

            OnRotation?.Invoke(deltaAlongTangent * rotationCoefficient);
        }
    }

    private void OnMouseLeftClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        if (isRotatable)
        {
            Vector2 distanceNormal = -clickEvent.Position.normalized;
            Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

            float deltaAlongTangent = Vector2.Dot(clickEvent.Delta, distanceTangent);

            OnRotation?.Invoke(deltaAlongTangent * rotationCoefficient);
        }
    }
}
