using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public struct OneFingerSlideEvent
    {
        public Vector2 Position;
        public Vector2 PositionCentered;
        public Vector2 Delta;
    }

    public struct TwoFingerSlideEvent
    {
        public OneFingerSlideEvent FirstFinger;
        public OneFingerSlideEvent SecondFinger;
    }

    public struct TapEvent
    {
        public Vector2 Position;
        public GameObject TappedObject;
    }

    public static event Action<OneFingerSlideEvent> OnOneFingerSlide;
    public static event Action<TwoFingerSlideEvent> OnTwoFingerSlide;
    public static event Action<TapEvent> OnTap;

    private bool isSliding;
    private Vector2 screenHalf;

    protected override void Awake()
    {
        base.Awake();

        isSliding = false;
        screenHalf = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        if (Input.touchCount > 1)
        {
            handleTwoFingerSlide();
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                isSliding = true;
                handleOneFingerSlide(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isSliding)
                {
                    handleTap(touch);
                }
                else
                { 
                    isSliding = false;
                }
            }
        }
    }

    private void handleOneFingerSlide(Touch touch)
    {
        OneFingerSlideEvent slideEvent = new OneFingerSlideEvent
        {
            Position = touch.position,
            PositionCentered = touch.position - screenHalf,
            Delta = touch.deltaPosition
        };

        OnOneFingerSlide?.Invoke(slideEvent);
    }

    private void handleTwoFingerSlide()
    {
        Touch touchOne = Input.GetTouch(0);
        Touch touchTwo = Input.GetTouch(1);

        TwoFingerSlideEvent slideEvent = new TwoFingerSlideEvent
        {
            FirstFinger = new OneFingerSlideEvent
            {
                Position = touchOne.position,
                PositionCentered = touchOne.position - screenHalf,
                Delta = touchOne.deltaPosition
            },
            SecondFinger = new OneFingerSlideEvent
            {
                Position = touchTwo.position,
                PositionCentered = touchTwo.position - screenHalf,
                Delta = touchTwo.deltaPosition
            }
        };

        OnTwoFingerSlide?.Invoke(slideEvent);
    }

    private void handleTap(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        GameObject tappedObject = null;
        if (Physics.Raycast(ray, out hit))
        {
            tappedObject = hit.collider.gameObject;
        }

        TapEvent tapEvent = new TapEvent
        {
            Position = touch.position,
            TappedObject = tappedObject
        };

        OnTap?.Invoke(tapEvent);
    }
}
