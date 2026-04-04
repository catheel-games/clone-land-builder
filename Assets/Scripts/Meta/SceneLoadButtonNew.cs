using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class SceneLoadButtonNew : MonoBehaviour
{
    [SerializeField] private UIButton button;
    [SerializeField] private string sceneName;
    
    void OnEnable()
    {
        button.OnUp += OnClick;
    }
    
    void OnDisable()
    {
        button.OnUp -= OnClick;
    }

    private void OnClick()
    {
        LoadManager.Instance.LoadScene(sceneName);
    }
}
