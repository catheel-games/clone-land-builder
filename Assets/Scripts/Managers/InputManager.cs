using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public struct OneFingerSlideEvent
    {
        public Vector2 Position;
        public Vector2 Delta;
    }

    public struct TwoFingerSlideEvent
    {
        public OneFingerSlideEvent FirstFinger;
        public OneFingerSlideEvent SecondFinger;
    }

    public struct MouseClickEvent
    {
        public Vector2 Position;
        public Vector2 Delta;
    }

    public static event Action<OneFingerSlideEvent> OnOneFingerSlide;
    public static event Action<TwoFingerSlideEvent> OnTwoFingerSlide;
    public static event Action<MouseClickEvent> OnMouseLeftClickSlide;
    public static event Action<MouseClickEvent> OnMouseRightClickSlide;

    void Update()
    {
        handleOneFingerSlide();
        handleTwoFingerSlide();
        handleMouseLeftClick();
        handleMouseRightClick();
    }

    private void handleOneFingerSlide()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            OneFingerSlideEvent slideEvent = new OneFingerSlideEvent
            {
                Position = touch.position,
                Delta = touch.deltaPosition
            };

            OnOneFingerSlide?.Invoke(slideEvent);
        }
    }

    private void handleTwoFingerSlide()
    {
        if (Input.touchCount > 1)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            TwoFingerSlideEvent slideEvent = new TwoFingerSlideEvent
            {
                FirstFinger = new OneFingerSlideEvent
                {
                    Position = touchOne.position,
                    Delta = touchOne.deltaPosition
                },
                SecondFinger = new OneFingerSlideEvent
                {
                    Position = touchTwo.position,
                    Delta = touchTwo.deltaPosition
                }
            };

            OnTwoFingerSlide?.Invoke(slideEvent);
        }
    }

    private void handleMouseLeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            MouseClickEvent clickEvent = new MouseClickEvent
            {
                Position = Input.mousePosition,
                Delta = Input.mousePositionDelta
            };

            OnMouseLeftClickSlide?.Invoke(clickEvent);
        }
    }

    private void handleMouseRightClick()
    {
        if (Input.GetMouseButton(1))
        {
            MouseClickEvent clickEvent = new MouseClickEvent
            {
                Position = Input.mousePosition,
                Delta = Input.mousePositionDelta
            };

            OnMouseRightClickSlide?.Invoke(clickEvent);
        }
    }
}
