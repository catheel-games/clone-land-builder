using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Hexagons.Type[] borderTypes = new Hexagons.Type[6];
    [SerializeField] private Hexagons.Type centerType;
    [SerializeField] private float rotationLerpCoefficient = 10f;

    private int rotationOffset = 0;
    private float rotationOffsetDiscrete = 0f;
    private bool isPlaced = false;

    void Update()
    {
        if (!isPlaced)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, rotationOffsetDiscrete, 0f),
                rotationLerpCoefficient * Time.deltaTime
            );
        }
    }

    public void Rotate(float newRotationOffset)
    {
        if (!isPlaced)
        {
            rotationOffsetDiscrete = newRotationOffset;
            rotationOffset = (int)Mathf.Floor(-rotationOffsetDiscrete / 60f);
        }
    }

    public void Place()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            transform.rotation = Quaternion.Euler(0f, rotationOffsetDiscrete, 0f);
        }
    }

    public Hexagons.Type GetSide(int side) { 
        return borderTypes[Tools.Modulo(rotationOffset + side, 6)];
    }
}