using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TileQueueInterface : MonoBehaviour
{
    [SerializeField] private Transform[] tileSlots;
    [SerializeField] private GameObject whitePlatePrefab;

    [Header("Animation Values")]
    [SerializeField] private float[] tilePositionsX;
    [SerializeField] private float[] tileTargetPositionsX;
    [SerializeField] private float[] tileMovementDuration;
    [SerializeField] private float lastTileMovementDelay = 0.2f;

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
                Quaternion.identity,
                tileSlots[i]
            );

            newTile.gameObject.layer = LayerMask.NameToLayer("3D UI");

            GameObject whitePlate = Instantiate(
                whitePlatePrefab,
                tileSlots[i].position,
                Quaternion.identity,
                newTile.transform
            );
            
            whitePlate.layer = LayerMask.NameToLayer("3D UI");

            if (newTile.gameObject.GetComponent<EiffelTile>())
            {
                if (i != 0)
                    newTile.gameObject.GetComponent<EiffelTile>().ChangeInsideGenerator(true);
                else
                    newTile.gameObject.GetComponent<EiffelTile>().ChangeInsideGenerator(false);
            }

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

            int index = i;

            tileSlots[index].DOLocalMoveX(tilePositionsX[index], 0f);

            if (index == 2) {
                DOVirtual.DelayedCall (lastTileMovementDelay,
                    ()=> tileSlots[index].DOLocalMoveX(tileTargetPositionsX[index], tileMovementDuration[index])
                );
            }
            else
                tileSlots[index].DOLocalMoveX(tileTargetPositionsX[index], tileMovementDuration[index]);
        }
    }

    public void CheckTileLeft()
    {
        if (LevelControl.Instance.tileScore < 3)
        {
            int tileScore = LevelControl.Instance.tileScore;
            for (int i = 0; i < tileSlots.Length; i++) {
                tileSlots[i].gameObject.SetActive(true);
            }

            for (int i = tileScore; i < tileSlots.Length; i++) {
                tileSlots[i].gameObject.SetActive(false);
            }
        }
        else {
            for (int i = 0; i < tileSlots.Length; i++)
            {
                tileSlots[i].gameObject.SetActive(true);
            }
        }
    }
}
