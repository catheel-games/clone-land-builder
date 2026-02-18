using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Vector3 keepVector3 = new Vector3(-90, 0, 0);
    [SerializeField] private Axises rotationAxis = Axises.z;

    Vector3 rotationVector;

    public enum Axises
    {
        x, y, z
    }

    void LateUpdate()
    {
        Vector3 rotation = keepVector3;

        float cameraRot = LevelControl.Instance.CameraPivotRotation;

        switch (rotationAxis)
        {
            case Axises.x:
                rotation.x += cameraRot;
                break;

            case Axises.y:
                rotation.y += cameraRot;
                break;

            case Axises.z:
                rotation.z += cameraRot;
                break;
        }

        transform.localRotation = Quaternion.Euler(rotation);
    }
}
