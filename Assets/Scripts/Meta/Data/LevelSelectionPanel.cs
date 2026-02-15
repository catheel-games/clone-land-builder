using UnityEngine;
using TMPro;

public class LevelSelectionPanel : MonoBehaviour
{
    [SerializeField] private bool isCompletedLevelPanel = false;

    [SerializeField] private TextMeshProUGUI starText;
    [SerializeField] private TextMeshProUGUI infoStarText;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private GameObject newTypeTextObject;

    [SerializeField] private GameObject playedBottomPart;
    [SerializeField] private GameObject playedTopPart;

    [SerializeField] private GameObject notPlayedBottomPart;
    [SerializeField] private GameObject notPlayedTopPart;

    [SerializeField] private TextMeshProUGUI levelText1;
    [SerializeField] private TextMeshProUGUI levelText2;

    [SerializeField] private GameObject[] resources;

    public void SetValues(int starValue, int coinValue, bool played, bool[] resourcesInLevel)
    {
        starText.text = 0 + "/" + starValue; 
        infoStarText.text = starValue.ToString();
        coinText.text = "+" + coinValue.ToString();

        newTypeTextObject.SetActive(!played);

        playedBottomPart.SetActive(played);
        playedTopPart.SetActive(played);

        notPlayedBottomPart.SetActive(!played);
        notPlayedTopPart.SetActive(!played);

        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].SetActive(resourcesInLevel[i]);
        }
    }

    public void SetLevelIDText(int levelID)
    {
        levelText1.text = "Level " + levelID;
        levelText2.text = "Level " + levelID;
    }


    public void SetResourceValues(bool[] resourcesInLevel)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].SetActive(resourcesInLevel[i]);
        }
    }

    public void RestartLevel()
    {
        SaveLoadManager.Instance.RefreshLevelData();
        LoadManager.Instance.LoadScene("Game");
    }
}
