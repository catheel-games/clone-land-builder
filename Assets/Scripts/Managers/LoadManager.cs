using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Singleton<LoadManager>
{
    [SerializeField] private LoadingScreen loadingScreen;

    private AsyncOperation operation;

    public void LoadScene(string sceneName)
    {
        if (operation == null)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loadingScreen.Show();
        
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        do
        {
            loadingScreen.SetProgress(Mathf.Clamp01(operation.progress / 0.9f));
            yield return new WaitForSeconds(0.1f);
        }
        while (operation.progress < 0.9f);

        yield return new WaitForSeconds(0.6f);

        operation.allowSceneActivation = true;
        operation = null;

        loadingScreen.Hide();
    }
}
