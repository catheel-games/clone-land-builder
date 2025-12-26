using UnityEngine;

public abstract class UIContainer : MonoBehaviour
{
    [SerializeField] private UIContainer[] childContainers;

    private bool isActive;

    void Awake()
    {
        isActive = gameObject.activeSelf;
    }

    public virtual void Show()
    {
        if (!isActive)
        {
            isActive = true;
            gameObject.SetActive(true);
        }
    }

    public virtual void Hide()
    {
        if (isActive)
        {
            foreach (UIContainer child in childContainers)
            {
                if (child != null)
                {
                    child.Hide();
                }
            }
            
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}
