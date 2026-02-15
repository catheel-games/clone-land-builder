using UnityEngine;

public class TileExpansion : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;

    public void CheckExpansion(Hexagons.Coords coords)
    {
        CheckExpansionAtTile(coords);
        Hexagons.IterateNeighbours(coords, (side, neighbourCoords) => CheckExpansionAtTile(neighbourCoords));
    }

    private void CheckExpansionAtTile(Hexagons.Coords coords)
    {
        Tile tile = tileGrid.GetTile(coords);

        if (tile != null)
        {
            tile.ExpandCenter();
            
            Hexagons.IterateNeighbours(coords, (side, neighbourCoords) => {
                Tile neighbour = tileGrid.GetTile(neighbourCoords);

                if (neighbour != null)
                {
                    neighbour.ExpandSide(side, neighbour.GetSide(Tools.Modulo(side + 3, 6)));
                }
            });
        }
    }
}
