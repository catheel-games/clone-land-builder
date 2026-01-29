using UnityEngine;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    [SerializeField] private GameData gameData;

    public LevelDataSO[] levelDatas;

    private void Start()
    {
        gameData = GameData.Instance;
    }

    public void SaveData()
    {
        gameData = GameData.Instance;

        string json = JsonUtility.ToJson(gameData);
        Debug.Log("Saving: " + json);

        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            writer.Write(json);
        }
    }

    public void LoadData()
    {
        string json = string.Empty;

        using (StreamReader reader = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            json = reader.ReadToEnd();
        }

        GameData data = JsonUtility.FromJson<GameData>(json);
        gameData.SetData(data.levelsProgress);
    }

}



