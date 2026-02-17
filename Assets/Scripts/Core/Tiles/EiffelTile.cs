using UnityEngine;

public class EiffelTile : MonoBehaviour
{
    [SerializeField] private GameObject particleObject;

    void Start()
    {
        particleObject.SetActive(true);
        LevelControl.Instance._levelCanvasControl.OnTilePreviewControlClick += PlaceEiffelTile;
    }

    public void PlaceEiffelTile(bool isAccepted) 
    {
        if (isAccepted)
        {
            particleObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        LevelControl.Instance._levelCanvasControl.OnTilePreviewControlClick -= PlaceEiffelTile;
    }
}
