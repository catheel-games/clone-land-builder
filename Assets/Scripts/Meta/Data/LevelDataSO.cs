using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    [Header("Static Level Info")]
    public int levelIndex;
    public int goalValue;
    public int coinValue;

    public LevelReward reward;

    public bool[] levelTiles = {false, false, false, false, false};
    public enum LevelReward
    {
        ForestUnlocked,
        FieldUnlocked,
        FinishUnlocked
    }
}
