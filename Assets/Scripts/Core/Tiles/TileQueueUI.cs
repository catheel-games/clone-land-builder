using UnityEngine;
using System.Collections.Generic;

public class TileQueueUI : Singleton<TileQueueUI>
{
    [SerializeField] private Transform[] tileSlots;
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
            GameObject newTile = Instantiate(
                queue[i].gameObject,
                tileSlots[i].position,
                Quaternion.identity
            );
            newTile.layer = 3;
            displayedTiles.Add(newTile);
        }
    }
}
