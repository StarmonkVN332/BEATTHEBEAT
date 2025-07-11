using UnityEngine;

public class SwipeJump : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    public float minSwipeDistance = 50f;
    public float jumpForce = 7f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("JumpController yêu cầu Rigidbody");
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isSwiping = true;
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        endTouchPosition = touch.position;
                        DetectSwipe();
                        isSwiping = false;
                    }
                    break;
            }
        }
    }

    private void DetectSwipe()
    {
        float swipeDistanceY = endTouchPosition.y - startTouchPosition.y;
        float swipeDistanceX = Mathf.Abs(endTouchPosition.x - startTouchPosition.x);

        // Vuốt lên: khoảng cách Y lớn, và phải lớn hơn X
        if (swipeDistanceY > minSwipeDistance && swipeDistanceY > swipeDistanceX)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Chỉ nhảy khi đang đứng trên mặt đất (velocity.y gần 0)
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
