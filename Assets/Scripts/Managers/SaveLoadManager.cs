using UnityEngine;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    [SerializeField] private GameData gameData;

    public LevelDataSO[] levelDatas;
    [SerializeField] private int levelCount;

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
        Debug.Log("Creating Default Data!");

        gameData.levelsProgress.Clear();

/*        for (int i = 1; i <= levelCount; i++)
        {
            gameData.levelsProgress.Add(new LevelProgress
            {
                levelIndex = i,
                currentScore = 0,
                currentTilesLeft = 50,
                state = i == 1
                    ? LevelProgress.LevelState.Unlocked
                    : LevelProgress.LevelState.Blocked
            });
        }*/

        // Creating 3Level Test Default Data
            for (int i = 1; i <= levelCount; i++)
            {
                gameData.levelsProgress.Add(new LevelProgress
                {
                    levelIndex = i,
                    currentScore = 0,
                    currentTilesLeft = 50,
                    state = i switch
                    {
                        1 => LevelProgress.LevelState.Unlocked,
                        2 => LevelProgress.LevelState.Unlocked,
                        3 => LevelProgress.LevelState.Unlocked
                    }
                });
            }

        gameData.currentLevelID = 1;
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

    public void DeleteData() {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.LogWarning("No save file to delete.");
        }
    }

}



