using System;
using UnityEngine;

// thank you, Hayk <<3
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
    
    private int startIndex;
    private bool isChosen;
    private Type[] Roullette;
    void Awake() {
        startIndex = 0;
        isChosen = false;
        Roullette = (Type[])Enum.GetValues(typeof(Type));
    }

    public void Rotate(int step) {
        startIndex = MainUtilities.Modulo(startIndex + step, 6);
        transform.rotation = Quaternion.AngleAxis(startIndex * 60, Vector3.up);
    }

    public Type GetSideType(int side) { 
        return borderTypes[(startIndex + side) % 6];
    }

    public void ChooseTile() { 
        isChosen = true;    
    }

    public void TypeSetter(int i, Type type) {
        borderTypes[i] = type;
    }

    public void RandomSixTypeSetter() {
        for (int i = 0; i < 6; i++) {
            borderTypes[i] = (Type)Roullette.GetValue(UnityEngine.Random.Range(0,6));
        }
    }
}
