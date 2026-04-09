using UnityEngine;

public class Tile : MonoBehaviour
{
    private float rotationLerpCoefficient = 4f;
    [SerializeField] private Hexagons.Type centerType;
    [SerializeField] private Hexagons.Type[] borderTypes = new Hexagons.Type[6];
    [SerializeField] private GameObject centerExpansion;
    [SerializeField] private GameObject[] borderExpansions = new GameObject[6];

    private int rotationOffset = 0;
    private float rotationOffsetDiscrete = 0f;
    
    private bool isPlaced = false;
    private bool isCenterExpanded = false;
    private bool[] isBorderExpanded = new bool[6] {false, false, false, false, false, false};

    [SerializeField] private bool isUpgraded = false;

    public float RotationOffsetDiscrete => rotationOffsetDiscrete;
    public bool IsUpgraded => isUpgraded;

    private float originalRotation = 0f;
    
    void Update()
    {
        if (!isPlaced)
        {
            originalRotation = Mathf.Lerp(originalRotation, rotationOffsetDiscrete, rotationLerpCoefficient * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, originalRotation, 0f);
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
        //if (!isPlaced)
        {
            rotationOffsetDiscrete = newRotationOffsetDiscrete;
            rotationOffset = (int)Mathf.Floor(-rotationOffsetDiscrete / 60f);
            originalRotation = rotationOffsetDiscrete;
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