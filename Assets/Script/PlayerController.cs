using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 10f;      // tốc độ phản hồi kéo ngang
    [SerializeField] private float forwardSpeed = 5f;    // tốc độ chạy về phía trước
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;
    [SerializeField] private float smoothTime = 0.1f;    // thời gian làm mượt

    private Vector2 previousTouchPos;
    private float targetX;                               // vị trí x mục tiêu
    private float velocityX = 0f;                         // tốc độ tạm để smooth

    void Start()
    {
        targetX = transform.position.x;
    }

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

                // Tính toán delta x
                float deltaX = (delta.x / Screen.width) * moveSpeed;

                // Cập nhật target x với Clamp
                targetX = Mathf.Clamp(targetX + deltaX, minX, maxX);

                // Gọi animation nếu cần
                animator.SetTrigger("isMoving");
            }
        }

        // Làm mượt chuyển động theo trục X
        currentPosition.x = Mathf.SmoothDamp(currentPosition.x, targetX, ref velocityX, smoothTime);

        // Cập nhật vị trí mới
        transform.position = new Vector3(currentPosition.x, 0f, currentPosition.z);
    }
}
