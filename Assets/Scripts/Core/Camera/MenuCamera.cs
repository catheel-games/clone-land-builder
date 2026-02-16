using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float targetWorldWidth = 11f;
    [SerializeField] private Camera cam;


    void Start()
    {
        float aspect = (float)Screen.height / Screen.width;
        cam.orthographicSize = targetWorldWidth * 0.5f * aspect;
    }
}
