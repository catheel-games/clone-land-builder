using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class TileDecoration : MonoBehaviour
{
    [Serializable]
    public struct Decoration
    {
        public GameObject decorationPrefab;
        public Hexagons.Type tileType;
        [NonSerialized] public int spawnCount;
    }
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private Decoration[] decorations;
    
    public void SpawnDecoration(Hexagons.Coords hex, GameObject decoration)
    {
        Vector3 spawnPosition = Hexagons.HexToWorld(hex);
        GameObject newDecoration = Instantiate(decoration, spawnPosition, Quaternion.identity);

        Sequence seq = DOTween.Sequence();
        seq.Append(newDecoration.transform.DOScale(0, 0));
        seq.AppendInterval(2f);
        seq.Append(newDecoration.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
    }

    public void CheckTile(Hexagons.Coords coords)
    {
        for (int i = 0; i < decorations.Length; i++)
        {
            List<Hexagons.Coords> visited = new List<Hexagons.Coords>();
            int areaCount = CountConnectedTiles(coords, visited, decorations[i].tileType);

            int count = areaCount / 3;
            int countToSpawn = count - decorations[i].spawnCount;

            for (int j = 0; j < countToSpawn; j++)
            {
                SpawnDecoration(coords, decorations[i].decorationPrefab);
                decorations[i].spawnCount++;
            }
        }
    }

    private int CountConnectedTiles(Hexagons.Coords coords, List<Hexagons.Coords> visited, Hexagons.Type type)
    {
        Tile tile = tileGrid.GetTile(coords);
        visited.Add(coords);

        Hexagons.IterateNeighbours(coords, (side, neighborCoords) => {
            Tile neighbor = tileGrid.GetTile(neighborCoords);
            if (neighbor == null) return;
            Hexagons.Type neighbourType = Hexagons.Type.Null;
            Hexagons.Type instanceType = tile.GetSide(side);

            neighbourType = neighbor.GetSide(Tools.Modulo(side + 3, 6));

            if (visited.Contains(neighborCoords))
            {
                return;
            }
            else
            {
                if (instanceType == type && neighbourType == type)
                {
                    visited.Add(neighborCoords);
                    CountConnectedTiles(neighborCoords, visited, type);
                }
            }
        });

        return visited.Count;
    }
}