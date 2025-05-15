using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioSource slashSound;

    [SerializeField] private float moveSpeed = 10f; // Tốc độ phản hồi khi kéo
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;

    private Vector2 previousTouchPos;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Khi chạm lần đầu, lưu vị trí
            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPos = touch.position;
            }
            // Khi đang kéo
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - previousTouchPos;
                previousTouchPos = touch.position;

                // Tính delta X theo tỷ lệ màn hình để di chuyển
                float deltaX = (delta.x / Screen.width) * moveSpeed;

                // Cập nhật vị trí mới với giới hạn X, Y và Z giữ nguyên
                float newX = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
                transform.position = new Vector3(newX, 0f, -8f);

                // Kích hoạt animation nếu cần
                animator.SetTrigger("isMoving");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AnimationTrigger"))
        {
            animator.SetTrigger("isAttackRight");
            slashSound.pitch = Random.Range(0.8f, 1.2f);
            slashSound.Play();
            slashEffect.Play();
        }
    }
}
