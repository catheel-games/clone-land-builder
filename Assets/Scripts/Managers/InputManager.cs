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

    [SerializeField] private Camera cameraRef;

    private Vector2 screenCenter = new Vector2(0.5f, 0.5f * Screen.height / Screen.width);

    void Update()
    {
        handleOneFingerSlide();
        handleTwoFingerSlide();
        handleMouseLeftClick();
        handleMouseRightClick();
    }

    private Vector2 processInputPosition(Vector2 positionInput)
    {
        return positionInput / Screen.width - screenCenter;
    }

    private Vector2 processInputDelta(Vector2 deltaInput)
    {
        return deltaInput / Screen.width;
    }

    private void handleOneFingerSlide()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            OneFingerSlideEvent slideEvent = new OneFingerSlideEvent
            {
                Position = processInputPosition(touch.position),
                Delta = processInputDelta(touch.deltaPosition)
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
                    Position = processInputPosition(touchOne.position),
                    Delta = processInputDelta(touchOne.deltaPosition)
                },
                SecondFinger = new OneFingerSlideEvent
                {
                    Position = processInputPosition(touchTwo.position),
                    Delta = processInputDelta(touchTwo.deltaPosition)
                }
            };

            OnTwoFingerSlide?.Invoke(slideEvent);
        }
    }

    private void handleMouseLeftClick()
    {
        if (Input.GetMouseButton(0) && Input.touchCount == 0)
        {
            MouseClickEvent clickEvent = new MouseClickEvent
            {
                Position = processInputPosition(Input.mousePosition),
                Delta = processInputDelta(Input.mousePositionDelta)
            };

            OnMouseLeftClickSlide?.Invoke(clickEvent);
        }
    }

    private void handleMouseRightClick()
    {
        if (Input.GetMouseButton(1) && Input.touchCount == 0)
        {
            MouseClickEvent clickEvent = new MouseClickEvent
            {
                Position = processInputPosition(Input.mousePosition),
                Delta = processInputDelta(Input.mousePositionDelta)
            };

            OnMouseRightClickSlide?.Invoke(clickEvent);
        }
    }
}
