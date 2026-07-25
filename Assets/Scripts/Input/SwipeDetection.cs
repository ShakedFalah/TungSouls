using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public enum SwipeDirection
{
    Left,
    Right,
    Up,
    Down
}

public class SwipeDetection : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference touchPress;
    [SerializeField] private InputActionReference touchPosition;

    [Header("Swipe Settings")]
    [SerializeField] private float minSwipeDistance = 800f;
    [SerializeField] private float swipeCooldown = 0.2f;

    private float nextSwipeTime = 0;


    [SerializeField] private PlayerController playerController;

    public System.Action<SwipeDirection> OnSwipe;

    private Vector2 startPos;
    private bool isSwiping;

    private void OnEnable()
    {
        touchPress.action.started += StartSwipe;
        touchPress.action.canceled += EndSwipe;

        touchPress.action.Enable();
        touchPosition.action.Enable();
    }

    private void OnDisable()
    {
        touchPress.action.started -= StartSwipe;
        touchPress.action.canceled -= EndSwipe;

        touchPress.action.Disable();
        touchPosition.action.Disable();
    }

    private void StartSwipe(InputAction.CallbackContext ctx)
    {
        startPos = Touchscreen.current.primaryTouch.position.ReadValue();
        isSwiping = true;
        nextSwipeTime = 0;
    }

    private void EndSwipe(InputAction.CallbackContext ctx)
    {
        isSwiping = false;
    }

    private void Update()
    {
        if (!isSwiping)
            return;

        Vector2 currentPos = touchPosition.action.ReadValue<Vector2>();
        Vector2 delta = currentPos - startPos;

        if (delta.magnitude < minSwipeDistance)
        {
            return;
        }

        if (Time.time < nextSwipeTime)
        {
            return;
        }

        switch (HandleSwipe(delta))
        {
            case SwipeDirection.Right:
                playerController.MoveRight();
                break;
            case SwipeDirection.Left:
                playerController.MoveLeft();
                break;
            case SwipeDirection.Up:
                playerController.Jump();
                break;
        }

        startPos = currentPos;
        nextSwipeTime = Time.time + swipeCooldown;
    }



    private SwipeDirection HandleSwipe(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                return SwipeDirection.Right;
            }

            return SwipeDirection.Left;
        }
        if (dir.y > 0)
        {
            return SwipeDirection.Up;
        }

        return SwipeDirection.Down;
    }
}