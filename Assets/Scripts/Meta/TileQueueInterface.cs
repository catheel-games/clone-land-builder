using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TileQueueInterface : MonoBehaviour
{
    [SerializeField] private Transform[] tileSlots;
    [SerializeField] private GameObject whitePlatePrefab;

    private List<GameObject> displayedTiles = new List<GameObject>();
    private Tile firstTile;

    public Tile FirstTile() => firstTile;

    public void UpdateDisplay(List<Tile> queue)
    {
        foreach (GameObject tile in displayedTiles)
        {
            Destroy(tile);
        }

        displayedTiles.Clear();

        for (int i = 0; i < tileSlots.Length && i < queue.Count; i++)
        {

            Tile newTile = Instantiate(
                queue[i],
                tileSlots[i].position,
                Quaternion.identity
            );

            newTile.gameObject.layer = LayerMask.NameToLayer("3D UI");

            GameObject whitePlate = Instantiate(
                whitePlatePrefab,
                tileSlots[i].position,
                Quaternion.identity,
                newTile.transform
            );
            
            whitePlate.layer = LayerMask.NameToLayer("3D UI");

            displayedTiles.Add(newTile.gameObject);

            if (i == 0)
            {
                firstTile = newTile;
                newTile.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.1f);
            }
            else
            {
                newTile.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            }
        }
    }
}
