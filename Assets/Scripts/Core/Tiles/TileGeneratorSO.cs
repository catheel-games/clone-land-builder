using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TileGeneratorSO", menuName = "Scriptable Objects/TileGeneratorSO")]
public class TileGeneratorSO : ScriptableObject
{
    public int levelIndex;

    public FullTiles fullTiles;
    public TypeTiles typeTiles;

}

    [Serializable]
    public class FullTiles
    {
        public Tile[] fullGrassTiles;
        public Tile[] fullWaterTiles;
        public Tile[] fullTownTiles;
        public Tile[] fullYellowTiles;
        public Tile[] fullForestTiles;
    }

    [Serializable]
    public class TypeTiles
    {
        public Tile[] grassTiles;
        public Tile[] waterTiles;
        public Tile[] townTiles;
        public Tile[] yellowTiles;
        public Tile[] forestTiles;
    }

