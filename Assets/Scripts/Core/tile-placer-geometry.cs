using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float tileSize = 1f;

    [Header("References")]
    [SerializeField] private GameObject tilePrefab;

    void Start()
    {
        MakeMapGrid();
    }

    private Vector2 GetHexCoords(int x, int z)
    {
        float xPos = x * tileSize * Mathf.Cos(Mathf.Deg2Rad * 30f);

        float zPos = z * tileSize + ((x % 2 == 1) ? tileSize * 0.5f : 0f);

        return new Vector2(xPos, zPos);
    }

    private void MakeMapGrid()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                Vector2 hexCoords = GetHexCoords(x, z);

                Vector3 worldPosition = new Vector3(hexCoords.x, 0f, hexCoords.y);

                Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
            }
        }
    }
}

