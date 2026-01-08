using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIContainer : MonoBehaviour
{
    [SerializeField] private UIContainer[] childContainers;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] private bool activeMode = false;

    protected bool isActive;

    void Awake()
    {
        setVisibility(activeMode);
    }

    public virtual void Reset()
    {
        setVisibility(activeMode);

        foreach (UIContainer child in childContainers)
        {
            child.Reset();
        }
    }

    public virtual void Show()
    {
        if (!isActive)
        {
            Reset();
            setVisibility(true);
        }
    }

    public virtual void Hide()
    {
        if (isActive)
        {
            setVisibility(false);
        }
    }

    private void setVisibility(bool visible)
    {
        isActive = visible;
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
