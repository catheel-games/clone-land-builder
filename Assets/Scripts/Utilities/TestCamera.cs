using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private float zoomCoefficient = 0.01f;
    [SerializeField] private float rotationCoefficient = 0.01f;
    [SerializeField] private float lerpCoefficient = 5f;

    private void calcZoomAndWorldRotate(Vector3 firstPosition, Vector3 secondPosition, Vector3 firstDelta, Vector3 secondDelta, out float zoomAmount, out float rotationAmount)
    {
        Vector2 firstDistanceNormal = (secondPosition - firstPosition).normalized;
        Vector2 firstDistanceTangent = new Vector2(-firstDistanceNormal.y, firstDistanceNormal.x);

        float firstDeltaAlongNormal = Vector2.Dot(firstDelta, firstDistanceNormal);
        float firstDeltaAlongTangent = Vector2.Dot(firstDelta, firstDistanceTangent);   
        Vector2 secondDistanceNormal = -firstDistanceNormal;
        Vector2 secondDistanceTangent = -firstDistanceTangent;

        float secondDeltaAlongNormal = Vector2.Dot(secondDelta, secondDistanceNormal);
        float secondDeltaAlongTangent = Vector2.Dot(secondDelta, secondDistanceTangent);

        zoomAmount = firstDeltaAlongNormal + secondDeltaAlongNormal;
        rotationAmount = firstDeltaAlongTangent + secondDeltaAlongTangent;
    }

    private void calcObjectRotate(Vector3 positionCentered, Vector3 delta, out float rotationAmount)
    {
        Vector2 positionNormal = -positionCentered.normalized;
        Vector2 positionTangent = new Vector2(-positionNormal.y, positionNormal.x);

        float deltaAlongTangent = Vector2.Dot(delta, positionTangent);

        rotationAmount = deltaAlongTangent;
    }
}
