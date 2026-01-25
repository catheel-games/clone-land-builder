using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Singleton<LoadManager>
{
    [SerializeField] private LoadingScreenContainer loadingScreenContainer;

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
        loadingScreenContainer.Show();
        loadingScreenContainer.SetProgress(0);
        loadingScreenContainer.TitleScaling();

        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float minLoadTime = 1.25f;
        float elapsedTime = 0f;

        float displayedProgress = 0f;
        float fakeSpeed = 1.5f;

        float stopPercent = Random.Range(0.87f, 0.93f);
        int randomDisplaying;

        while (operation.progress < 0.9f || elapsedTime < minLoadTime)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            float targetProgress = Mathf.Min(realProgress, stopPercent);

            displayedProgress = Mathf.MoveTowards (displayedProgress, targetProgress, fakeSpeed * Time.deltaTime);

            randomDisplaying = Random.Range(0, 7);
            if (randomDisplaying == 0 || displayedProgress >= stopPercent)
                loadingScreenContainer.SetProgress(Mathf.Round(displayedProgress * 100f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.15f);

        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards (displayedProgress, 1f, fakeSpeed * Time.deltaTime);

            loadingScreenContainer.SetProgress(Mathf.Round(displayedProgress * 100f));
            yield return null;
        }

        loadingScreenContainer.StopAllCoroutines();

        operation.allowSceneActivation = true;
        operation = null;

        loadingScreenContainer.Hide();
    }
}
