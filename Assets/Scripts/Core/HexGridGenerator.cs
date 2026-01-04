using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float tileSize = 1f;

    [Header("References")]
    [SerializeField] private GameObject tilePrefab;

    const float sqrt3 = 1.73205080757f;
    const float sqrt3half = 0.86602540378f;

    void Start()
    {
        MakeMapGrid();
    }

    private Vector3 GetHexCoords(int x, int z)
    {
        Vector3 position = Vector3.zero;

        position += tileSize * new Vector3(sqrt3 * x, 0f, 1.5f * z);
        position += tileSize * new Vector3(sqrt3half * MainUtilities.Modulo(z, 2), 0f, 0f);

        return position;
    }

    private void MakeMapGrid()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                Instantiate(tilePrefab, GetHexCoords(x, z), Quaternion.identity, transform);
            }
        }
    }
}

