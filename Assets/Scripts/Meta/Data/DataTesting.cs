using UnityEngine;

public class DataTesting : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.Instance.LoadData();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoadManager.Instance.SaveData();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveLoadManager.Instance.DeleteData();
        }
    }
}
