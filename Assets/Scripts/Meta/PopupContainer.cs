using UnityEngine;

public class PopupContainer : FadingContainer
{
    [SerializeField] private UIContainer topBar;

    public override void Show()
    {
        base.Show();
        topBar.Hide();
    }

    public override void Hide()
    {
        base.Hide();
        topBar.Show();
    }
}
