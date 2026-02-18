using UnityEngine;
using DG.Tweening;

public class EiffelTile : MonoBehaviour
{
    [SerializeField] private GameObject particleObject;
    [SerializeField] private EiffelBlueSphere eiffelBlueSphere;

    private Hexagons.Coords eiffelCoords;

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnSubscribeFromEvents();
    }

    public void AcceptEiffelTile(bool isAccepted) 
    {
        if (isAccepted)
        {
            eiffelCoords = LevelControl.Instance.eiffelCoords;

            particleObject.SetActive(false);
            eiffelBlueSphere.gameObject.SetActive(true);
            eiffelBlueSphere.FlyToTile(eiffelCoords);

            DOVirtual.DelayedCall(1f, ()=> LevelControl.Instance.settingEiffel = false);

            UnSubscribeFromEvents();
        }
    }

    public void SetEiffelTile(Hexagons.Coords coords) 
    {
        LevelControl.Instance.eiffelCoords = coords;
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
