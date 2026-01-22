using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TileQueueUI : Singleton<TileQueueUI>
{
    [SerializeField] private Transform[] tileSlots;
    [SerializeField] private GameObject whitePlatePrefab;
    private List<GameObject> displayedTiles = new List<GameObject>();

    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (GameObject tile in displayedTiles)
            Destroy(tile);
        displayedTiles.Clear();

        List<Tile> queue = TileGenerator.Instance.GetQueue();

        if (queue.Count == 0) return;

        for (int i = 0; i < tileSlots.Length && i < queue.Count; i++)
        {
            GameObject whitePlate = Instantiate(
                whitePlatePrefab,
                tileSlots[i].position + new Vector3(0, -0.1f, 0),
                Quaternion.identity
            );
            whitePlate.transform.localScale = new Vector3(1.1f, 1f, 1f);
            whitePlate.layer = 3;
            displayedTiles.Add(whitePlate);

            GameObject newTile = Instantiate(
                queue[i].gameObject,
                tileSlots[i].position,
                Quaternion.identity
            );
            newTile.layer = 3;
            displayedTiles.Add(newTile);

            if (i == 0)
            {
                newTile.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.1f); 
                whitePlate.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.1f);
            }
            else
            {
                newTile.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                whitePlate.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            }
        }
    }
}
