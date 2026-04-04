using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor =  Color.white;
    [SerializeField] private float fadeDuration = 0.1f;
    
    private bool isPressed;
    private bool isHovered;
    private Sequence fadeSequence;
    
    public event Action OnDown;
    public event Action OnHold;
    public event Action OnUp;
    
    void Start()
    {
        isPressed = false;
        isHovered = false;
    }

    void Update()
    {
        if (isPressed)
        {
            OnHold?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        FadeToHighlighted();
        OnDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        FadeToNormal();

        if (isHovered)
        {
            OnUp?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
    
    private void FadeToNormal()
    {
        if (fadeSequence != null)
        {
            fadeSequence.Kill();
        }
        
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(targetImage.DOColor(normalColor, fadeDuration));
    }

    private void FadeToHighlighted()
    {
        if (fadeSequence != null)
        {
            fadeSequence.Kill();
        }
        
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(targetImage.DOColor(highlightedColor, fadeDuration));
    }
}
