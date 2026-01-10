using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;
    
    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        LoadManager.Instance.LoadScene(sceneName);
    }
}
