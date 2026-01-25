using System;

public class TilePreviewControlContainer : UIContainer
{
    public event Action<bool> OnControlClick;

    public void AcceptTile()
    {
        OnControlClick?.Invoke(true);
    }

    public void DeclineTile()
    {
        OnControlClick?.Invoke(false);
    }
}
