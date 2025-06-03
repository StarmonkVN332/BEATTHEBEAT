using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 10f; // tốc độ phản hồi khi kéo sang trái/phải
    [SerializeField] private float forwardSpeed = 5f; // tốc độ chạy về phía trước
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;

    private Vector2 previousTouchPos;

    void Update()
    {
        Vector3 currentPosition = transform.position;

        // Di chuyển về phía trước theo thời gian
        currentPosition.z += forwardSpeed * Time.deltaTime;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - previousTouchPos;
                previousTouchPos = touch.position;

                // Chỉ xử lý delta X
                float deltaX = (delta.x / Screen.width) * moveSpeed;
                currentPosition.x = Mathf.Clamp(currentPosition.x + deltaX, minX, maxX);

                // Gọi animation nếu cần
                animator.SetTrigger("isMoving");
            }
        }

        // Cập nhật vị trí mới
        transform.position = new Vector3(currentPosition.x, 0f, currentPosition.z);
    }
}
