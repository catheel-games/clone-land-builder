using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<UIContainer> mainContainers;

    private UIContainer currentContainer;

    protected override void Awake()
    {
        base.Awake();

        if (mainContainers.Count > 0)
        {
            currentContainer = mainContainers[0];
            currentContainer.Show();
        }
    }

    public void Transition(int index)
    {
        if (currentContainer != mainContainers[index])
        {
            if (currentContainer != null)
            {
                currentContainer.Hide();
            }
            
            currentContainer = mainContainers[index];
            currentContainer.Show();
        }
    }
}
