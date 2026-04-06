using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Vector2 screenCenter = new Vector2(0.5f, 0.5f * Screen.height / Screen.width);

    private bool inputIsEnabled = true;
    
    public bool InputIsEnabled => inputIsEnabled;

    void Update()
    {
        if (inputIsEnabled)
        {
            handleOneFingerSlide();
            handleTwoFingerSlide();
            handleMouseLeftClick();
            handleMouseRightClick();
        }
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
            
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;
            
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

            if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId)) return;
            if (EventSystem.current.IsPointerOverGameObject(touchTwo.fingerId)) return;
            
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
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
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
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            MouseClickEvent clickEvent = new MouseClickEvent
            {
                Position = processInputPosition(Input.mousePosition),
                Delta = processInputDelta(Input.mousePositionDelta)
            };

            OnMouseRightClickSlide?.Invoke(clickEvent);
        }
    }

    public void DisableInput() {
        inputIsEnabled = false;
    }

    public void EnableInput() {
        inputIsEnabled = true;
    }
}
