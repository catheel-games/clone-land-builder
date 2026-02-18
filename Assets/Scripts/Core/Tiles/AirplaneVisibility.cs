using System;
using DG.Tweening;
using UnityEngine;

public class AirplaneVisibility : MonoBehaviour
{
    public event Action OnDisappear;
    private Tween disappearTimer;

    void OnBecameInvisible()
    {
        disappearTimer = DOVirtual.DelayedCall(5f, () => OnDisappear?.Invoke());
    }

    void OnBecameVisible()
    {
        disappearTimer?.Kill();
    }
}