using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float minSwipeDistance = 80f;

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
        startPos = touchPosition.action.ReadValue<Vector2>();
        isSwiping = true;
    }

    private void EndSwipe(InputAction.CallbackContext ctx)
    {
        if (!isSwiping)
            return;

        Vector2 endPos = touchPosition.action.ReadValue<Vector2>();
        Vector2 delta = endPos - startPos;

        isSwiping = false;

        if (delta.magnitude < minSwipeDistance)
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
            default:
                break;
        }
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