using UnityEngine;
using DG.Tweening;

public class EiffelTile : MonoBehaviour
{
    [SerializeField] private GameObject particleObject;

    public void ChangeInsideGenerator(bool isInside)
    {
        if (isInside)
            UnSubscribeFromEvents();
        else
            SubscribeToEvents();
    }

    public void AcceptEiffelTile(bool isAccepted) 
    {
        if (isAccepted)
        {
            particleObject.SetActive(false);

            DOVirtual.DelayedCall(1f, ()=> LevelControl.Instance.settingEiffel = false);
        }
    }

    public void SetEiffelTile(Hexagons.Coords coords) 
    {
        LevelControl.Instance.settingEiffel = true;
    }


    private void OnDestroy()
    {
        UnSubscribeFromEvents();
    }

    public void SubscribeToEvents()
    {
        particleObject.SetActive(true);
        LevelControl.Instance._levelCanvasControl.OnTilePreviewControlClick += AcceptEiffelTile;
        LevelControl.Instance._tileControl.OnTilePlacerClick += SetEiffelTile;
    }

    public void UnSubscribeFromEvents()
    {
        particleObject.SetActive(false);
        LevelControl.Instance._levelCanvasControl.OnTilePreviewControlClick -= AcceptEiffelTile;
        LevelControl.Instance._tileControl.OnTilePlacerClick -= SetEiffelTile;
    }
}
