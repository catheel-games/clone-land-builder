using UnityEngine;

public class TileGenerator : Singleton<TileGenerator>
{
    [SerializeField] private Tile[] allTilePrefabs;

    public Tile GetRandomTile()
    {
        return Instantiate(
            allTilePrefabs[Random.Range(0, allTilePrefabs.Length)],
            Vector3.zero,
            Quaternion.identity
        );
    }
}
