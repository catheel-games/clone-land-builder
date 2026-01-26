using System;

public class TilePreviewControlContainer : UIContainer
{
    public event Action<bool> OnControlClick;

    public void AcceptTile()
    {
        OnControlClick?.Invoke(true);
        AudioManager.Instance.PlaySound("Tile", "Tile Accepting");
    }

    public void DeclineTile()
    {
        OnControlClick?.Invoke(false);
        AudioManager.Instance.PlaySound("Tile", "Tile Denying");
    }
}
