using UnityEngine;
using DG.Tweening;

public class EiffelBlueSphere : MonoBehaviour
{
    [SerializeField] private int destinationRange = 3;
    [SerializeField] private GameObject breakParticleObject;

    private Vector3 destinationCoordinates;
    private Hexagons.Coords newCoords;

    public void FlyToTile(Hexagons.Coords coords)
    {
        InputManager.Instance.DisableInput();

        int offsetX = Random.Range(-destinationRange, destinationRange);
        int offsetY = Random.Range(-destinationRange, destinationRange);

        if (offsetX == 0 && offsetY == 0) {
            FlyToTile(coords);
            return;
        }

        newCoords = new Hexagons.Coords(
            coords.x + offsetX,
            coords.y + offsetY
        );

        destinationCoordinates = Hexagons.HexToWorld(newCoords);

        Debug.Log("oldCoords: " + coords.x + "," + coords.y);

        Debug.Log("destinationCoordinates: " + destinationCoordinates);
        Debug.Log("newCoords: " + newCoords.x + "," + newCoords.y);



        if (LevelControl.Instance._tileControl.Tilegrid.HexagonPutTilesFinding(newCoords))
        {
            FlyToTile(coords);
            return;
        }

        else if (LevelControl.Instance._tileControl.Tilegrid.HexagonFrontierFinding(newCoords))
        {
            Flying();
        }

        else {
            Debug.Log("Create a hexagon");
            Flying();
        }
    }

    public void Flying()
    {
        transform.DOJump(
            destinationCoordinates,
            jumpPower: 2.25f,
            numJumps: 1,
            duration: 2.5f
            )
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                LevelControl.Instance._cameraControl.LockPosition(destinationCoordinates);
                if (!GameData.Instance.blueSphereTutored)
                {
                    GameData.Instance.blueSphereTutored = true;
                    DOVirtual.DelayedCall(0.1f, () => LevelControl.Instance._ftue.BlueSphereTutorial());
                }

                Instantiate(breakParticleObject, transform.position, Quaternion.identity, transform.parent);
                transform.DOScale(0f, 0.2f).OnComplete(
                                            ()=> Destroy(gameObject));
            });

    }

}
