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
        Lake
    }

    [SerializeField] private Type[] borderTypes;
    [SerializeField] private Type centerType;
    
    private int startIndex;
    private bool isChosen;

    void Awake() {
        startIndex = 0;
        isChosen = false;
    }

    public void Rotate(int step) {
        startIndex = ((startIndex + step) % 6 + 6) % 6;
        transform.rotation = Quaternion.AngleAxis(startIndex * 60, Vector3.up);
    }

    public Type GetSideType(int side) { 
        return borderTypes[(startIndex + side) % 6];
    }

    public void ChooseTile() { 
        isChosen = true;    
    }
}
