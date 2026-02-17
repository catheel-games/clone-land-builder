using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float rotationLerpCoefficient = 10f;
    [SerializeField] private Hexagons.Type centerType;
    [SerializeField] private Hexagons.Type[] borderTypes = new Hexagons.Type[6];
    [SerializeField] private GameObject centerExpansion;
    [SerializeField] private GameObject[] borderExpansions = new GameObject[6];

    private int rotationOffset = 0;
    private float rotationOffsetDiscrete = 0f;
    
    private bool isPlaced = false;
    private bool isCenterExpanded = false;
    private bool[] isBorderExpanded = new bool[6] {false, false, false, false, false, false};
    
    public float RotationOffsetDiscrete => rotationOffsetDiscrete;
    
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

    public void RotateInstantly(float newRotationOffsetDiscrete)
    {    
        if (!isPlaced)
        {
            rotationOffsetDiscrete = newRotationOffsetDiscrete;
            rotationOffset = (int)Mathf.Floor(-rotationOffsetDiscrete / 60f);
            transform.rotation = Quaternion.Euler(0f, rotationOffsetDiscrete, 0f);
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

    public Hexagons.Type CenterType => centerType;

    public Hexagons.Type GetSide(int side) {
        return borderTypes[Tools.Modulo(rotationOffset + side, 6)];
    }

    public void ExpandCenter()
    {
        if (centerExpansion == null) return;
        if (isCenterExpanded) return;
        
        isCenterExpanded = true;
        centerExpansion.SetActive(true);
    }

    public void ExpandSide(int side, Hexagons.Type neighborType)
    {
        side = Tools.Modulo(rotationOffset + side, 6);

        if (borderExpansions[side] == null) return;
        if (isBorderExpanded[side]) return;
        if ((int)borderTypes[side] >= (int)neighborType) return;
        
        isBorderExpanded[side] = true;
        borderExpansions[side].SetActive(true);
    }
}