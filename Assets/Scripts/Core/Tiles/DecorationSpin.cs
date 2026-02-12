using UnityEngine;

public class DecorationSpin : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.right;
    [SerializeField] private float speed = 30f;

    void Update()
    {
        transform.Rotate(rotationAxis * speed * Time.deltaTime, Space.Self);
    }
}
