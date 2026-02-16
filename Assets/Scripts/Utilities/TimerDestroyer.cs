using UnityEngine;

public class TimerDestroyer : MonoBehaviour
{
    [SerializeField] private float destroyDelay;

    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
