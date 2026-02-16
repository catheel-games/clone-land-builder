using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(-90f, 0f, LevelControl.Instance.CameraPivotRotation);
    }
}
