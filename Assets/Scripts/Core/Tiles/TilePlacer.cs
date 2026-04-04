using System;
using System.Collections;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private RectTransform tilePlacerPlus;
    [SerializeField] private Animator anim;

    private bool isAnimated = false;

    private Hexagons.Coords coords;

    public event Action<Hexagons.Coords> OnClick;

    void LateUpdate()
    {
        tilePlacerPlus.localRotation = Quaternion.Euler(0f, 0f, -LevelControl.Instance.CameraPivotRotation);
    }

    public void Setup(Hexagons.Coords coords, bool isFirstTile)
    {
        this.coords = coords;
        
        transform.position = Hexagons.HexToWorld(coords);

        isAnimated = isFirstTile;
    }

    private void OnEnable()
    {
        StartCoroutine(AnimationSetting());

    }

    public void Click()
    {
        OnClick?.Invoke(coords);
        VibrationManager.Instance.Vibrate();
        anim.enabled = false;
    }

    IEnumerator AnimationSetting() {
        anim.enabled = false;
        yield return new WaitForSeconds(1f);
        anim.enabled = isAnimated;
    }
}
