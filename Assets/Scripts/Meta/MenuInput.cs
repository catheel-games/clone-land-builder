using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuInput : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject levelSelectionPanel;
    public GameObject completedLevelSelectionPanel;

    [Header("Main UI")]
    public GameObject settingsIcon;
    public GameObject backIcon;
    public GameObject levelIcon;

    private GameState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = GameState.Menu;

        settingsPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
        completedLevelSelectionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Loadlevel(string levelName)
    {
        SceneManager.LoadScene("Level");

        // load corresponding level with the corresponding data
    }

    public void EnablePanel(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                if (menuPanel) menuPanel.SetActive(true);
                if (settingsIcon) settingsIcon.SetActive(true);
        }
            
    }
}