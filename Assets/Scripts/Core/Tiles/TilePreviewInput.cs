using UnityEngine;

public class TilePreviewInputInterpreter : MonoBehaviour
{
    [SerializeField] private TilePreview tilePreview;
    
    [SerializeField] private float rotationCoefficient = 0.5f;

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
        Vector2 distanceNormal = -slideEvent.Position.normalized;
        Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

        float deltaAlongTangent = Vector2.Dot(slideEvent.Delta, distanceTangent);

        tilePreview.ChangeRotation(deltaAlongTangent * rotationCoefficient);
    }

    private void OnMouseLeftClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        Vector2 distanceNormal = -clickEvent.Position.normalized;
        Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

        float deltaAlongTangent = Vector2.Dot(clickEvent.Delta, distanceTangent);

        tilePreview.ChangeRotation(deltaAlongTangent * rotationCoefficient);
    }
}
