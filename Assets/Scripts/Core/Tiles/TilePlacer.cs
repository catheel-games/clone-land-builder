using System;
using DG.Tweening;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private RectTransform tilePlacerPlus;
    [SerializeField] private Animator animator;

    private Hexagons.Coords coords;

    public event Action<Hexagons.Coords> OnClick;

    void LateUpdate()
    {
        tilePlacerPlus.localRotation = Quaternion.Euler(0f, 0f, -LevelControl.Instance.CameraPivotRotation);
    }

    public void Setup(Hexagons.Coords coords, bool isAnimated)
    {
        this.coords = coords;
        transform.position = Hexagons.HexToWorld(coords);
        
        Sequence animationSetup = DOTween.Sequence();
        
        animationSetup.AppendCallback(() => animator.enabled = false);
        animationSetup.AppendInterval(1f);
        animationSetup.AppendCallback(() => animator.enabled = isAnimated);
    }

    public void Click()
    {
        if (InputManager.Instance.InputIsEnabled)
        {
            animator.enabled = false;
            OnClick?.Invoke(coords);
            VibrationManager.Instance.Vibrate();
        }
    }
}
