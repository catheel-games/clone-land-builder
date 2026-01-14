using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Null,
        Town,
        Grass,
        Forest,
        Yellow,
        Water    
    }

    [SerializeField] private Type[] borderTypes;
    [SerializeField] private Type centerType;

    private Hexagons.Coords coords;
    private int rotationOffset;

    void Awake() {
        rotationOffset = 0;
    }

    public void SetCoords(Hexagons.Coords coords)
    {
        this.coords = coords;
    }

    public void Rotate(int step) {
        rotationOffset = Tools.Modulo(rotationOffset + step, 6);
        transform.rotation = Quaternion.AngleAxis(rotationOffset * 60, Vector3.up);
    }

    public Type GetSideType(int side) { 
        return borderTypes[(rotationOffset + side) % 6];
    }

    public void TypeSetter(int i, Type type) {
        borderTypes[i] = type;
    }
}
