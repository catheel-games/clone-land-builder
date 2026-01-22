using Unity.VisualScripting;
using UnityEngine;

public  class TileCalculator : MonoBehaviour
{
    [SerializeField] private float elementElevation = 1.0f;
    [SerializeField] private float starRadius = 0.6f;

    [SerializeField] private TilePreviewStar starPrefab;
    [SerializeField] private TilePreviewCombo comboPrefab;
    [SerializeField] private TileGrid tileGrid; // idk

    private TilePreviewStar[] stars = new TilePreviewStar[6];
    private TilePreviewCombo combo;

    public void CalculateBonuses(Hexagons.Coords coords, Tile tile)
    {
        int combinationAmount = 0;

        Hexagons.IterateNeighbours(coords, (side, neighborCoords) => {
            Tile neighbor = tileGrid.GetTile(neighborCoords);
            Hexagons.Type neighbourType = Hexagons.Type.Null;
            Hexagons.Type instanceType = tile.GetSide(side);

            if (neighbor != null)
            {
                neighbourType = neighbor.GetSide(Tools.Modulo(side + 3, 6));
            }

            if (neighbourType == instanceType)
            {
                combinationAmount += 1;
             
                if (stars[side] == null)
                {
                    float angle = side * 60f + 30f;

                    TilePreviewStar newStar = Instantiate(
                        starPrefab,
                        transform
                    );

                    newStar.transform.SetParent(transform, false);

                    newStar.transform.localPosition = Vector3.up * elementElevation + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * starRadius;

                    newStar.transform.localRotation = Quaternion.Euler(0f, angle, 0f);

                    stars[side] = newStar;
                }
            } 
            else
            {
                if (stars[side] != null)
                {
                    Destroy(stars[side].gameObject);
                    stars[side] = null;
                }
            }
        });

        if (combo != null)
        {
            Destroy(combo.gameObject);
            combo = null;
        }

        if (combinationAmount >= 3)
        {
            combo = Instantiate(
                comboPrefab,
                transform
            );

            combo.transform.localPosition = new Vector3(0f, elementElevation, 0f);
        }
    }

    public void ProcessBonsuses()
    {
        foreach(TilePreviewStar star in stars)
        {
            if (star != null)
            {
                Destroy(star.gameObject);
            }
        }

        if (combo != null)
        {
            Destroy(combo.gameObject);
        }
    }
}
