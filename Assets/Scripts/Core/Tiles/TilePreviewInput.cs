using UnityEngine;

public class TilePreviewInputInterpreter : Singleton<TilePreviewInputInterpreter>
{   
    [SerializeField] private float rotationCoefficient = 0.5f;
    private TilePreview tilePreviewInstance;

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
        if (tilePreviewInstance == null) return;

        Vector2 distanceNormal = -slideEvent.Position.normalized;
        Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

        float deltaAlongTangent = Vector2.Dot(slideEvent.Delta, distanceTangent);

        tilePreviewInstance.ChangeRotation(deltaAlongTangent * rotationCoefficient);
    }

    private void OnMouseLeftClickSlide(InputManager.MouseClickEvent clickEvent)
    {
        if (tilePreviewInstance == null) return;

        Vector2 distanceNormal = -clickEvent.Position.normalized;
        Vector2 distanceTangent = new Vector2(-distanceNormal.y, distanceNormal.x);

        float deltaAlongTangent = Vector2.Dot(clickEvent.Delta, distanceTangent);

        tilePreviewInstance.ChangeRotation(deltaAlongTangent * rotationCoefficient);
    }

    public void SetTilePreview(TilePreview preview)
    {
        tilePreviewInstance = preview;
    }
}
