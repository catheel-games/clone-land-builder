using UnityEngine;

public class Rotate : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, 40 * Time.deltaTime);
    }
}
