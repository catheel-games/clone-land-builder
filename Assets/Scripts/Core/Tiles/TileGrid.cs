using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private UIContainer tilePlacerContainer;
    [SerializeField] private TilePlacer tilePlacerPrefab;

    [SerializeField] private TileDenyingGridFeedback tileDenyingGridFeedback;
    [SerializeField] private TileAcceptingGridFeedback tileAcceptingGridFeedback;

    private Dictionary<Hexagons.Coords, TilePlacer> frontier = new Dictionary<Hexagons.Coords, TilePlacer>();
    private Dictionary<Hexagons.Coords, Tile> tiles = new Dictionary<Hexagons.Coords, Tile>();

    public event Action<Hexagons.Coords> OnTilePlacerClick;
    public event Action OnTilePlace;

    private Vector3 centerCoordinates;

    private int tileGridDiameter = 1;
    private Vector3 tileGridCenter = Vector3.zero;
    
    public int TileGridDiameter => tileGridDiameter;
    public Vector3 TileGridCenter => tileGridCenter;
    public UIContainer TilePlacerContainer => tilePlacerContainer;
    
    void Start()
    {
        SetTilePlacer(new Hexagons.Coords(0, 0), true);
    }
    
    private void SetTilePlacer(Hexagons.Coords coords, bool isFirstTile = false)
    {
        if (!frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer newTilePlacer = Instantiate(tilePlacerPrefab, tilePlacerContainer.transform);
                newTilePlacer.Setup(coords, isFirstTile);
                newTilePlacer.OnClick += Lock;
                frontier.Add(coords, newTilePlacer);
            }
        }
    }

    public void SetFakeTilePlacer(Hexagons.Coords coords, TilePlacer fakePrefab)
    {
        TilePlacer newTilePlacer = Instantiate(fakePrefab, tilePlacerContainer.transform);
        newTilePlacer.Setup(coords, false);
    }

    public void SetTile(Hexagons.Coords coords, Tile tile)
    {
        if (frontier.ContainsKey(coords))
        {
            if (!tiles.ContainsKey(coords))
            {
                TilePlacer oldTilePlacer = frontier[coords];
                oldTilePlacer.OnClick -= Lock;
                Destroy(oldTilePlacer.gameObject);
                frontier.Remove(coords);

                tiles.Add(coords, tile);
                OnTilePlace?.Invoke();
                
                tile.transform.SetParent(transform, false);
                
                Sequence sequence = DOTween.Sequence();
                
                sequence.Append(tile.transform.DOMoveY(0f, 0.2f));
                sequence.Append(tile.transform.DOMoveY(0.014f, 0.05f));
                sequence.Append(tile.transform.DOMoveY(0f, 0.01f));
                sequence.AppendCallback(() => tile.transform.position = Hexagons.HexToWorld(coords));
                
                tile.transform.position = Hexagons.HexToWorld(coords);
                
                UpdateTileGridCenterAndDiameter();

                if (SaveLoadManager.Instance.gameData.ftueIsEnded)
                {
                    Hexagons.IterateNeighbours(coords, (int side, Hexagons.Coords neighbor) =>
                    {
                        SetTilePlacer(neighbor);
                    });
                }

                else if (!SaveLoadManager.Instance.gameData.ftueIsEnded)
                {
                    switch (LevelControl.Instance.fTUEtileID) {
                        case 1:
                            SetTilePlacer(new Hexagons.Coords(1, 0));
                            break;
                        case 2:
                            SetTilePlacer(new Hexagons.Coords(Tools.Modulo(0, 2), 1));
                            break;
                    }
                }
            }
        }
    }

    public void TileSetFTUEEnded()
    {
        for (int i = 0; i < tiles.Count; i++) {
            foreach (KeyValuePair<Hexagons.Coords, Tile> tile in tiles) {
                Hexagons.IterateNeighbours(tile.Key, (int side, Hexagons.Coords neighbor) =>
                {
                    SetTilePlacer(neighbor);
                });
            }
        }
    }
    
    public Tile GetTile(Hexagons.Coords coords)
    {
        tiles.TryGetValue(coords, out Tile tile);
        return tile;
    }

    public (Hexagons.Type type, int count) GetLeastOccurringSide(Hexagons.Type excludeType = Hexagons.Type.Null)
    {
        int[] sideCounts = new int[6];
        int smallestCount = int.MaxValue;
        int smallestIndex = 1;

        foreach (Hexagons.Coords frontierCoord in frontier.Keys)
        {
            Hexagons.IterateNeighbours(frontierCoord, (int side, Hexagons.Coords neighbor) => {
                if(tiles.ContainsKey(neighbor))
                {
                    Tile neighborTile = GetTile(neighbor);
                    if (neighborTile != null)
                    {
                        Hexagons.Type neighborType = neighborTile.GetSide(Tools.Modulo(side + 3, 6));
                        sideCounts[(int)neighborType]++;
                    }
                }
            });
        }

        for (int i = 1; i < sideCounts.Length; i++)
        {
            if ((Hexagons.Type)i == excludeType) continue;

            if (sideCounts[i] < smallestCount)
            {
                smallestCount = sideCounts[i];
                smallestIndex = i;
            }
        }

        return ((Hexagons.Type)smallestIndex, smallestCount);
    }

    private void Lock(Hexagons.Coords coords)
    {
        tileAcceptingGridFeedback.Activate(frontier);
        tilePlacerContainer.Hide();
        OnTilePlacerClick?.Invoke(coords);
    }

    public void Unlock()
    {
        tileDenyingGridFeedback.Activate(frontier);
        tilePlacerContainer.Show();
    }

    public void ReplaceTile(Hexagons.Coords coords, Tile newTile)
    {
        Tile oldTile = tiles[coords];
        Destroy(oldTile.gameObject);
        newTile.transform.SetParent(transform, false);
        newTile.transform.position = Hexagons.HexToWorld(coords);
        tiles[coords] = newTile;
    }
    
    public Vector3 GetRandomTilePosition()
    {
        var keys = new List<Hexagons.Coords>(tiles.Keys);
        return Hexagons.HexToWorld(keys[Random.Range(0, keys.Count)]);
    }

    public bool HexagonFrontierFinding(Hexagons.Coords targetCoords)
    {
        return frontier.ContainsKey(targetCoords);
    }

    public bool HexagonPutTilesFinding(Hexagons.Coords targetCoords)
    {
        return tiles.ContainsKey(targetCoords);
    }

    private void UpdateTileGridCenterAndDiameter()
    {
        // i am gonna do the cheapest trick in history
        // by using the only thing i remember from the probability class
        // i am gonna calculate variance of a data set or something

        Vector2 linearSum = Vector2.zero;
        Vector2 quadraticSum = Vector2.zero;
        int count = tiles.Count;
        
        foreach (Hexagons.Coords coords in tiles.Keys)
        {
            Vector3 worldCoords = Hexagons.HexToWorld(coords);
            
            linearSum += new Vector2(worldCoords.x, worldCoords.z);
            quadraticSum += new Vector2(worldCoords.x * worldCoords.x, worldCoords.z *  worldCoords.z);
        }
        
        Vector2 linearAverage = linearSum / count;
        Vector2 quadraticAverage = quadraticSum / count;
        
        Vector2 variance = quadraticAverage - new Vector2(linearAverage.x * linearAverage.x, linearAverage.y * linearAverage.y);

        tileGridCenter = new Vector3(linearAverage.x, 0f, linearAverage.y);
        tileGridDiameter = Mathf.FloorToInt(Mathf.Sqrt(variance.magnitude) * 2 + 1);
    }
    
}