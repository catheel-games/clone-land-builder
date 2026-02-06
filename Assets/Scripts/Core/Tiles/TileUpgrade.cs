using UnityEngine;
using System;
using DG.Tweening;

public class TileUpgrade : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private Tile[] cityTilePrefabs;
    [SerializeField] private Tile[] bigCityTilePrefabs;

    public void Upgrade(Hexagons.Coords coords, Tile upgradePrefab)
    {
        Tile newTile = Instantiate(upgradePrefab, tileGrid.transform);
        tileGrid.ReplaceTile(coords, newTile);
        newTile.Place();

        Sequence seq = DOTween.Sequence();
        seq.Append(newTile.transform.DOScaleY(0, 0)).Append(newTile.transform.DOScaleY(1, 0.3f).SetEase(Ease.OutBack));
    }

    public void CheckUpgrade(Hexagons.Coords coords)
    {
        UpgradeCount(coords);
        Hexagons.IterateNeighbours(coords, (side, neighborCoords) =>
        {
            Tile neighbor = tileGrid.GetTile(neighborCoords);
            if (neighbor == null) return;
            UpgradeCount(neighborCoords);
        });
    }

    private void UpgradeCount(Hexagons.Coords coords)
    {
        int count = 0;
        Tile tile = tileGrid.GetTile(coords);

        Hexagons.IterateNeighbours(coords, (side, neighborCoords) => {
            Tile neighbor = tileGrid.GetTile(neighborCoords);
            if (neighbor == null) return;
            Hexagons.Type neighbourType = Hexagons.Type.Null;
            Hexagons.Type instanceType = tile.GetSide(side);

            neighbourType = neighbor.GetSide(Tools.Modulo(side + 3, 6));

            if (instanceType == Hexagons.Type.Town && neighbourType == Hexagons.Type.Town)
            {
                count++;
            }
        });

        if (count == 4 || count == 5)
        {
            Upgrade(coords, cityTilePrefabs[UnityEngine.Random.Range(0, cityTilePrefabs.Length)]);
        }
        else if (count == 6)
        {
            Upgrade(coords, bigCityTilePrefabs[UnityEngine.Random.Range(0, bigCityTilePrefabs.Length)]);
        }
    }
}
