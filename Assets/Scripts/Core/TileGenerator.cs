using Unity.VisualScripting;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] hexagon;
    [SerializeField] private GameObject DefaultPrefab;
    [SerializeField] private Transform[] LevelParent;  //parent transform object for the hexagons
    [SerializeField] private Camera cam;
    [SerializeField] private int chosenLevel;
    [SerializeField] private int tileoffset; //how far one tile is from another, x axis
    private Vector3[] VisibleHexPosition;
    private Vector3 HexPosition;
    [SerializeField] private Vector3 YAxisOffset;  //for the game designer, about how low the tile blocks should be placed UI wise.
    private int count;
    private Quaternion DummyQuaternion;
    public void Generate() {
        count = hexagon.Length;
        Vector3 offset = new Vector3(tileoffset, 0, 0); //vectorized tile offset
        VisibleHexPosition = new Vector3[3];
        VisibleHexPosition[0] = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, cam.nearClipPlane)); //first hex position, x axis center from the screen
        VisibleHexPosition[1] = VisibleHexPosition[0] + offset;
        VisibleHexPosition[2] = VisibleHexPosition[1] + offset;
        HexPosition = VisibleHexPosition[2] + new Vector3(Screen.width / 2, 0, 0);  //not visible stack of hexagons out of the screen
        DummyQuaternion = Quaternion.identity;
        if (hexagon == null) {
            count = 50;
            hexagon = new GameObject[count];
            for (int i = 0; i < hexagon.Length; i++) {
                hexagon[i] = DefaultPrefab;
            }
        }
        if (LevelParent == null) { 
            LevelParent = new Transform[1];
            chosenLevel = 0;
        }
        
        for (int i = 0; i < count; i++) {
            if (i < 3)
            {
                Instantiate(hexagon[i], YAxisOffset + VisibleHexPosition[i], DummyQuaternion, LevelParent[chosenLevel]);
            }
            else {
                Instantiate(hexagon[i], YAxisOffset + HexPosition, DummyQuaternion, LevelParent[chosenLevel]);
            }

        }

    }
}
