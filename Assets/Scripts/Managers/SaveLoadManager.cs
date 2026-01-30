using UnityEngine;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    [SerializeField] private GameData gameData;

    public LevelDataSO[] levelDatas;

    private string SavePath =>
    Path.Combine(Application.persistentDataPath, "SaveData.json");

    private void Start()
    {
        gameData = GameData.Instance;

        if (File.Exists(SavePath))
        {
            LoadData();
        }
        else
        {
            CreateDefaultData();
            SaveData();
        }
    }

    private void CreateDefaultData()
    {
        gameData.levelsProgress.Clear();

        LevelProgress progress = new LevelProgress
        {
            levelIndex = 1,
            currentScore = 0,
            currentTilesLeft = 50,
            state = LevelProgress.LevelState.Unlocked
        };

        gameData.levelsProgress.Add(progress);
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        Debug.Log("Saving: " + json);

        File.WriteAllText(SavePath, json);
    }

    public void LoadData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Save file not found. Creating default data.");
            CreateDefaultData();
            SaveData();
            return;
        }

        string json = File.ReadAllText(SavePath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        gameData.SetData(data.levelsProgress);
    }

}



