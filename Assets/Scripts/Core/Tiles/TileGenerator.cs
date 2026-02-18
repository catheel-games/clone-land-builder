using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] TileGeneratorSO[] tileGeneratorSO;
    [SerializeField] private Tile tileFTUE;
    [SerializeField] private Tile eiffelTile;
    [SerializeField] private float eiffelTileChance;

    [SerializeField] private float random1TileChance = 40;
    [SerializeField] private float random2TileChance = 15;

    private TileGeneratorSO currentGenerator;
    private bool eiffelIsSpawned = false;

    List<Tile> setQueue = new List<Tile>();

    private Hexagons.Type ignoredType = Hexagons.Type.Null;
    private int ignoreCounter = 0;
    private int tilesPlaced = 0;

    public Func<Hexagons.Type, (Hexagons.Type type, int count)> OnLeastTypeRequest;

    void Awake()
    {
        currentGenerator = tileGeneratorSO[GameData.Instance.currentLevelID - 1];

        if (GameData.Instance.ftueIsEnded)
            InitializeQueue();
        else if (!GameData.Instance.ftueIsEnded)
            InitializeFTUEQueue();
    }

    public void InitializeQueue()
    {
        Hexagons.Type type1 = GetRandomType();
        Hexagons.Type type2 = GetRandomTypeExcluding(type1);
        setQueue.AddRange(CreateTypedSet(type1));
        setQueue.AddRange(CreateTypedSet(type2));

        // For quick Testing
        // setQueue.Add(eiffelTile);
    }

    public void InitializeFTUEQueue()
    {
        Hexagons.Type hexType = Hexagons.Type.Grass;
        setQueue.AddRange(CreateFTUETypedSet(hexType));
    }

    public void InitializeEiffelTileQueue()
    {
        setQueue.Insert(0, eiffelTile);
        LevelControl.Instance._tileControl.SetEiffelTowerGenerator();
    }

    private Tile GetRandomTile()
    {
        float uniqueTileChance = UnityEngine.Random.Range(0, 100);

        Debug.Log(uniqueTileChance);

        if (!eiffelIsSpawned && uniqueTileChance <= eiffelTileChance && GameData.Instance.eiffelIsUnlocked)
        {
            eiffelIsSpawned = true;
            return eiffelTile;
        }

        Hexagons.Type randomType = GetRandomType();
        Tile randomTile = UnityEngine.Random.Range(0, 2) == 0 ? GetFullTile(randomType) : GetTypeTile(randomType);

        return randomTile;
    }

    private Hexagons.Type GetRandomType()
    {
        return (Hexagons.Type)UnityEngine.Random.Range(1, 6);
    } 

    private (Hexagons.Type type, int count) GetLeastOccurringType(Hexagons.Type excludeType = Hexagons.Type.Null)
    {
        return OnLeastTypeRequest?.Invoke(excludeType) ?? (Hexagons.Type.Grass, 0);
    }

    private Hexagons.Type GetRandomTypeExcluding(Hexagons.Type firstType)
    {
        Hexagons.Type secondType = GetRandomType();
        if (secondType == firstType)
            return GetRandomTypeExcluding(firstType);
        return secondType;
    }

    private Tile GetFullTile(Hexagons.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Hexagons.Type.Grass:
                arr = currentGenerator.fullTiles.fullGrassTiles;
                break;
            case Hexagons.Type.Water:
                arr = currentGenerator.fullTiles.fullWaterTiles;
                break;
            case Hexagons.Type.Town:
                arr = currentGenerator.fullTiles.fullTownTiles;
                break;
            case Hexagons.Type.Yellow:
                arr = currentGenerator.fullTiles.fullYellowTiles;
                break;
            case Hexagons.Type.Forest:
                arr = currentGenerator.fullTiles.fullForestTiles;
                break;
            default:
                arr = currentGenerator.fullTiles.fullGrassTiles;
                break;
        }

        if (arr.Length == 0)
            return GetFullTile(GetRandomType());

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    private Tile GetTypeTile(Hexagons.Type type)
    {
        Tile[] arr;
        switch (type)
        {
            case Hexagons.Type.Grass:
                arr = currentGenerator.typeTiles.grassTiles;
                break;
            case Hexagons.Type.Water:
                arr = currentGenerator.typeTiles.waterTiles;
                break;
            case Hexagons.Type.Town:
                arr = currentGenerator.typeTiles.townTiles;
                break;
            case Hexagons.Type.Yellow:
                arr = currentGenerator.typeTiles.yellowTiles;
                break;
            case Hexagons.Type.Forest:
                arr = currentGenerator.typeTiles.forestTiles;
                break;
            default:
                arr = currentGenerator.typeTiles.grassTiles;
                break;
        }

        if (arr.Length == 0)
            return GetTypeTile(GetRandomType());

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    private List<Tile> CreateTypedSet(Hexagons.Type type)
    {
        List<Tile> newSet = new List<Tile>();
        newSet.Add(GetFullTile(type));
        Tile typed1 = GetTypeTile(type);
        Tile typed2 = GetTypeTile(type);
        while (typed2 == typed1)
        {
            typed2 = GetTypeTile(type);
        }
        newSet.Add(typed1);
        newSet.Add(typed2);
        newSet = newSet.OrderBy(x => UnityEngine.Random.value).ToList();
        return newSet;
    }

    private List<Tile> CreateFTUETypedSet(Hexagons.Type type)
    {
        List<Tile> newSet = new List<Tile>();

        newSet.Add(GetFullTile(type));
        newSet.Add(tileFTUE);
        newSet.Add(GetFullTile(type));

        return newSet;
    }

    private List<Tile> CreateRandomSet(Hexagons.Type type)
    {
        List<Tile> randomSet = new List<Tile>();
        randomSet.Add(GetFullTile(type));
        Tile typed1 = GetTypeTile(type);
        Tile typed2 = GetTypeTile(type);
        while (typed2 == typed1)
        {
            typed2 = GetTypeTile(type);
        }
        randomSet.Add(typed1);
        randomSet.Add(typed2);

        randomSet = randomSet.OrderBy(x => UnityEngine.Random.value).ToList();

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand <= random2TileChance)
        {
            Tile random1 = GetRandomTile();
            Tile random2 = GetRandomTile();
            while (random2 == random1)
            {
                random2 = GetRandomTile();
            }
            randomSet.Add(random1);
            randomSet.Add(random2);
        }
        else if (rand <= random1TileChance)
        {
            randomSet.Add(GetRandomTile());
        }

        return randomSet;
    }

    private void RefillQueue()
    {
        if (setQueue.Count < 3)
        {
            var leastType = GetLeastOccurringType();

            if (tilesPlaced >= 15 && leastType.count <= 2 && ignoreCounter == 0)
            {
                ignoredType = leastType.type;
                ignoreCounter = 8;
            }

            Hexagons.Type typeToUse;
            if(ignoreCounter > 0 && leastType.type == ignoredType)
            {
                typeToUse = GetLeastOccurringType(ignoredType).type;
            }
            else
            {
                typeToUse = leastType.type;
            }

            setQueue.AddRange(CreateRandomSet(typeToUse));
        }
    }

    public Tile ShowNextTile()
    {
        return setQueue[0];
    }

    public Tile GetNextTile()
    {
        Tile prevTile = setQueue[0];
        setQueue.RemoveAt(0);

        tilesPlaced++;
        if (ignoreCounter > 0) ignoreCounter--;

        RefillQueue();
        return prevTile;
    }

    public List<Tile> GetQueue()
    {
        return setQueue;
    }
}