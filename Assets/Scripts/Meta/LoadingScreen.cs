using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen :FadingContainer
{
    [SerializeField] private Slider progressBar;

    public void SetProgress(float progress)
    {
        progressBar.value = progress;
    }
}
