using UnityEngine;

public class CameraInputInterpreter : MonoBehaviour
{
    [SerializeField] private CameraController controllerRef;
    [SerializeField] private Camera cameraRef;

    [SerializeField] private float zoomCoefficient = 2f;
    [SerializeField] private float rotationCoefficient = 0.5f;
    [SerializeField] private float translationCoefficient = 4f;

    private Vector2 screenCenter = new Vector2(Screen.width, Screen.height);

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
        Vector3 touchPositionA = cameraRef.ScreenToWorldPoint(slideEvent.Position - slideEvent.Delta);
        Vector3 touchPositionB = cameraRef.ScreenToWorldPoint(slideEvent.Position);
        
        controllerRef.ChangePosition((touchPositionB - touchPositionA) * translationCoefficient);
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

        controllerRef.ChangeRotation((firstDeltaAlongTangent + secondDeltaAlongTangent) * rotationCoefficient);
        controllerRef.ChangeZoom((firstDeltaAlongNormal + secondDeltaAlongNormal) * zoomCoefficient);
    }

    private void OnMouseLeftClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        Vector3 mousePositionA = cameraRef.ScreenToWorldPoint(clickEvent.Position - clickEvent.Delta);
        Vector3 mousePositionB = cameraRef.ScreenToWorldPoint(clickEvent.Position);

        controllerRef.ChangePosition((mousePositionB - mousePositionA) * translationCoefficient);
    }

    private void OnMouseRightClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        Vector2 firstDistanceNormal = (clickEvent.Position - screenCenter).normalized;
        Vector2 firstDistanceTangent = new Vector2(-firstDistanceNormal.y, firstDistanceNormal.x);

        float firstDeltaAlongNormal = Vector2.Dot(clickEvent.Delta, firstDistanceNormal);
        float firstDeltaAlongTangent = Vector2.Dot(clickEvent.Delta, firstDistanceTangent);   

        controllerRef.ChangeRotation(firstDeltaAlongTangent * rotationCoefficient);
        controllerRef.ChangeZoom(firstDeltaAlongNormal * zoomCoefficient);
    }
}
