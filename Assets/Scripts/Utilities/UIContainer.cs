using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIContainer : MonoBehaviour
{
    [SerializeField] private UIContainer[] childContainers;
    [SerializeField] protected CanvasGroup canvasGroup;

    private bool isActive;

    void Awake()
    {
        isActive = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void Show()
    {
        if (!isActive)
        {
            isActive = true;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public virtual void Hide()
    {
        if (isActive)
        {
            foreach (UIContainer child in childContainers)
            {
                child.Hide();
            }
            
            isActive = false;
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
