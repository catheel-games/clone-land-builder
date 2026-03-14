using UnityEngine;
using DG.Tweening;

public class EiffelBlueSphere : MonoBehaviour
{
    [SerializeField] private int destinationRange = 3;
    [SerializeField] private GameObject breakParticleObject;
    [SerializeField] private GameObject ftuePing;
    [SerializeField] private TilePlacer fakeTilePlacer;

    public void FlyToTile(Hexagons.Coords coords)
    {
        if (!SaveLoadManager.Instance.gameData.progressData.blueSphereTutored)
            InputManager.Instance.DisableInput();

        Hexagons.Coords newCoords;
        Vector3 destinationCoordinates;

        while (true)
        {
            int offsetX = Random.Range(-destinationRange, destinationRange);
            int offsetY = Random.Range(-destinationRange, destinationRange);

            if (offsetX == 0 && offsetY == 0)
                continue;

            newCoords = new Hexagons.Coords(
                coords.x + offsetX,
                coords.y + offsetY
            );

            if (LevelControl.Instance._tileControl.Tilegrid.HexagonPutTilesFinding(newCoords))
            {
                continue;
            }
            else if (LevelControl.Instance._tileControl.Tilegrid.HexagonFrontierFinding(newCoords))
            {
                Debug.Log("Frontier");
            }
            else
            {
                DOVirtual.DelayedCall(2.5f, () => LevelControl.Instance._tileControl.Tilegrid.SetFakeTilePlacer(newCoords, fakeTilePlacer));
            }

            break;
        }

        destinationCoordinates = Hexagons.HexToWorld(newCoords);

        Debug.Log("oldCoords: " + coords.x + "," + coords.y);

        Debug.Log("destinationCoordinates: " + destinationCoordinates);
        Debug.Log("newCoords: " + newCoords.x + "," + newCoords.y);

        Flying(destinationCoordinates);
    }

    public void Flying(Vector3 destination)
    {
        transform.DOJump(
            destination,
            jumpPower: 2.25f,
            numJumps: 1,
            duration: 2.5f
            )
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (!SaveLoadManager.Instance.gameData.progressData.blueSphereTutored)
                {
                    LevelControl.Instance._cameraControl.LockPosition(destination);
                    SaveLoadManager.Instance.gameData.progressData.blueSphereTutored = true;
                    DOVirtual.DelayedCall(0.1f, () => LevelControl.Instance._ftue.BlueSphereTutorial());
                }

                AudioManager.Instance.PlaySound("Plus", "Paris Blue Sphere");

                Instantiate(breakParticleObject, transform.position, Quaternion.identity, transform.parent);
                Instantiate(ftuePing, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);

                transform.DOScale(0f, 0.2f).OnComplete(
                                            ()=> Destroy(gameObject));
            });

    }

}
