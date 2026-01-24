using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TileQueueInterface : MonoBehaviour
{
    [SerializeField] private Transform[] tileSlots;
    [SerializeField] private GameObject whitePlatePrefab;

    private List<GameObject> displayedTiles = new List<GameObject>();

    public void UpdateDisplay(List<Tile> queue)
    {
        foreach (GameObject tile in displayedTiles)
        {
            Destroy(tile);
        }

        displayedTiles.Clear();

        for (int i = 0; i < tileSlots.Length && i < queue.Count; i++)
        {
            GameObject whitePlate = Instantiate(
                whitePlatePrefab,
                tileSlots[i].position + new Vector3(0, 0, 0),
                Quaternion.identity,
                tileSlots[i].transform
            );

            whitePlate.transform.localScale = new Vector3(1.1f, 1f, 1f);
            whitePlate.layer = LayerMask.NameToLayer("3D UI");
            displayedTiles.Add(whitePlate);

            GameObject newTile = Instantiate(
                queue[i].gameObject,
                tileSlots[i].position,
                Quaternion.identity,
                whitePlate.transform
            );

            newTile.layer = LayerMask.NameToLayer("3D UI");
            displayedTiles.Add(newTile);

            if (i == 0)
            {
                whitePlate.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.1f);
            }
            else
            {
                whitePlate.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            }
        }
    }
}
