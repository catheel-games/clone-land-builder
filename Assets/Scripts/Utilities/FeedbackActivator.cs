using UnityEngine;

public class FeedbackActivator : MonoBehaviour
{
    [SerializeField] private TilePlacementFeedback tilePlacementFeedback;

    void Update()
    {
        int pressedNumber = GetPressedNumberKey();
        switch (pressedNumber)
        {
            case 1:
                tilePlacementFeedback.ActivateFeedback();
                break;
            case 2:
                Debug.Log("No feedback for Tile Accepting");
                break;
            case 3:
                Debug.Log("No feedback for Tile Denying");
                break;
            case 4:
                Debug.Log("No feedback for Tile Rotation");
                break;
            case 5:
                tilePlacementFeedback.DestroyFeedback();
                break;
        }
    }

    int GetPressedNumberKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;

        return -1;
    }
}
