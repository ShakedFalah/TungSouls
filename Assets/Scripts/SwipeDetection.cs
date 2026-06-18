using Unity.VisualScripting;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public float minSwipeDistance = 10f;
    Vector2 touchStart;
    Vector2 touchEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            } else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeVector = touch.position - touchStart;

                Vector2 absVector = swipeVector.Abs();

                if (Mathf.Max(absVector.x, absVector.y) < minSwipeDistance)
                {
                    return;
                }

                if (absVector.x > absVector.y)
                {
                    Debug.Log("Horizontal Swipe");
                    if (swipeVector.x > 0)
                    {
                        Debug.Log("Right swipe");
                    } else
                    {
                        Debug.Log("Left swipe");
                    }
                } else
                {
                    Debug.Log("Vertical Swipe");
                    if (swipeVector.y > 0)
                    {
                        Debug.Log("Up swipe");
                    }
                }
            }
        }
    }
}
