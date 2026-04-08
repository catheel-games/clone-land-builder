using System;

public class TilePreviewControlContainer : UIContainer
{
    public event Action<bool> OnControlClick;

    public void AcceptTile()
    {
        OnControlClick?.Invoke(true);

        if (LevelControl.Instance.setting5StarTile)
            AudioManager.Instance.PlaySound("Tile", "Tile Accepting in Stars");
        else
            AudioManager.Instance.PlaySound("Tile", "Tile Accepting");
    }

    public void DeclineTile()
    {
        OnControlClick?.Invoke(false);
        AudioManager.Instance.PlaySound("Tile", "Tile Denying");
    }
}
