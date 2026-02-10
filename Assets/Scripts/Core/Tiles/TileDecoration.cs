using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using System.Text.RegularExpressions;

public class TileDecoration : MonoBehaviour
{
    [Serializable]
    public struct DecorationGroup
    {
        public GameObject[] prefabs;
        public Hexagons.Type tileType;
        public float spawnHeight;
    }

    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private DecorationGroup[] decorationGroups;

    private void SpawnDecoration(Hexagons.Coords hex, DecorationGroup group, int groupIndex, int prefabIndex)
    {
        Vector3 spawnPosition = GetEdgeSpawnPosition(hex, group.tileType);
        spawnPosition.y = group.spawnHeight;
        GameObject newDecoration = Instantiate(group.prefabs[prefabIndex], spawnPosition, Quaternion.identity);

        DecorationMovement movement = newDecoration.GetComponent<DecorationMovement>();
        movement.GroupIndex = groupIndex;
        movement.PrefabIndex = prefabIndex;
        movement.SpawnCoord = hex;
        movement.TileType = group.tileType;
        movement.Grid = tileGrid;
        movement.Initialize(group.spawnHeight);

        Sequence seq = DOTween.Sequence();
        seq.Append(newDecoration.transform.DOScale(0, 0));
        seq.AppendInterval(2f);
        seq.Append(newDecoration.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
    }

    public void CheckTile(Hexagons.Coords coords)
    {
        for (int i = 0; i < decorationGroups.Length; i++)
        {
            List<Hexagons.Coords> visited = new List<Hexagons.Coords>();
            int areaCount = CountConnectedTiles(coords, visited, decorationGroups[i].tileType);

            int prefabIndex = DecorationPicker(visited, decorationGroups[i], i);

            int existingInRegion = 0;
            foreach (var dec in DecorationMovement.AllDecorations)
            {
                if (dec.GroupIndex == i && visited.Contains(dec.SpawnCoord))
                    existingInRegion++;
            }

            int count = areaCount / 3;
            int countToSpawn = count - existingInRegion;

            for (int j = 0; j < countToSpawn; j++)
            {
                SpawnDecoration(coords, decorationGroups[i], i, prefabIndex);
            }
        }
    }

    private int DecorationPicker(List<Hexagons.Coords> regionTiles, DecorationGroup group, int groupIndex)
    {
        foreach (DecorationMovement dec in DecorationMovement.AllDecorations)
        {
            if (regionTiles.Contains(dec.SpawnCoord) && dec.GroupIndex == groupIndex)
            {
                return dec.PrefabIndex;
            }
        }
        return UnityEngine.Random.Range(0, group.prefabs.Length);
    }

    private int CountConnectedTiles(Hexagons.Coords coords, List<Hexagons.Coords> visited, Hexagons.Type type)
    {
        Tile tile = tileGrid.GetTile(coords);
        if (tile == null) return visited.Count;
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
                    CountConnectedTiles(neighborCoords, visited, type);
                }
            }
        });

        return visited.Count;
    }

    private Vector3 GetEdgeSpawnPosition(Hexagons.Coords hex, Hexagons.Type tileType)
    {
        Tile tile = tileGrid.GetTile(hex);
        Vector3 spawnPos = Hexagons.HexToWorld(hex);

        Hexagons.IterateNeighbours(hex, (side, neighborCoords) => {
            Tile neighbor = tileGrid.GetTile(neighborCoords);
            if (neighbor == null) return;
            Hexagons.Type neighbourType = Hexagons.Type.Null;
            Hexagons.Type instanceType = tile.GetSide(side);

            neighbourType = neighbor.GetSide(Tools.Modulo(side + 3, 6));

            if (tile.GetSide(side) == tileType)
            {
                Vector3 hexCenter = Hexagons.HexToWorld(hex);
                Vector3 neighborCenter = Hexagons.HexToWorld(neighborCoords);
                spawnPos = hexCenter + (neighborCenter - hexCenter) * 0.4f;
            }
        });

        return spawnPos;
    }
}
