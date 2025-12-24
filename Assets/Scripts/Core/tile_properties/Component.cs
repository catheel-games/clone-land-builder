using UnityEngine;

public class Component : MonoBehaviour
{
    public enum Types
    {
        Town,
        Grass,
        Forest,
        Lake,
        Null
    }
    [SerializeField] private Types type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Types GetType() {
        return type;
    }
    
}
