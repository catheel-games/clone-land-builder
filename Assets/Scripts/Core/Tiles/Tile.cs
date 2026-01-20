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

    private int rotationOffset;

    void Awake() {
        rotationOffset = 0;
    }

    public Type GetSideType(int side) { 
        return borderTypes[(rotationOffset + side) % 6];
    }

    public void TypeSetter(int i, Type type) {
        borderTypes[i] = type;
    }
}