using UnityEngine;

public class FeedbackActivator : MonoBehaviour
{
    [SerializeField] private TilePlacementFeedback tilePlacementFeedback;
    [SerializeField] private TileDenyingFeedback tileDenyingFeedback;

    private GameObject currentTileObject;

    [SerializeField] private GameObject tileObject;
    [SerializeField] private GameObject[] tilePlacementZones;

    void Update()
    {
        int pressedNumber = GetPressedNumberKey();
        switch (pressedNumber)
        {
            case 1:
                currentTileObject = tilePlacementFeedback.ActivateFeedback(tileObject, tilePlacementZones);
                break;
            case 2:
                tileDenyingFeedback.ActivateFeedback(currentTileObject, tilePlacementZones);
                break;
            case 3:
                Debug.Log("No feedback for Tile Accepting");
                break;
            case 4:
                Debug.Log("No feedback for Tile Rotation");
                break;
        }
    }

    int GetPressedNumberKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;

        return -1;
    }
}
