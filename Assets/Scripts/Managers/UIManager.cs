using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<UIContainer> mainContainers;

    private UIContainer currentContainer;

    protected override void Awake()
    {
        base.Awake();

        foreach (UIContainer main in mainContainers)
        {
            main.Hide();
        }

        if (mainContainers.Count > 0)
        {
            currentContainer = mainContainers[0];
            currentContainer.Show();
        }
    }

    public void Transition(int index)
    {
        if (index < 0) return;
        if (index >= mainContainers.Count) return;
        if (currentContainer == mainContainers[index]) return;

        if (currentContainer != null)
        {
            currentContainer.Hide();
        }
        
        currentContainer = mainContainers[index];
        currentContainer.Show();
    }
}
