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

    [SerializeField] private Collider collider;
    [SerializeField] private GameObject Continue_UI;
    [SerializeField] private AudioSource failSongPlayer;
    public static bool isGameOver = false;

    [Header("Model Rotation")]
    public GameObject rotationModel; // Gán GameObject visual (model) vào đây trong Inspector
    [SerializeField] private float rotationAngle = 20f;
    [SerializeField] private float rotationSmoothTime = 0.2f;
    [SerializeField] private float rotationVelocityThreshold = 0.1f; // Ngưỡng tốc độ ngang để xoay

    private float targetX;
    private float velocityX = 0f;

    private Vector2 previousTouchPos;

    private float targetYRotation = 0f;
    private float currentYVelocity = 0f;

    void Start()
    {
        targetX = transform.position.x;
        collider.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 🚫 Chờ đến khi note được spawn và nhạc bắt đầu
        if (!GameState.GameStarted)
            return;

        Vector3 currentPosition = transform.position;

        // Di chuyển về phía trước
        currentPosition.z += forwardSpeed * Time.deltaTime;

        // Điều khiển bằng cảm ứng
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

                float deltaX = (delta.x / Screen.width) * moveSpeed;
                targetX = Mathf.Clamp(targetX + deltaX, minX, maxX);
                animator.SetTrigger("isMoving");
            }
        }

        // Làm mượt di chuyển ngang
        currentPosition.x = Mathf.SmoothDamp(currentPosition.x, targetX, ref velocityX, smoothTime);

        // Áp dụng vị trí mới
        transform.position = new Vector3(currentPosition.x, 0f, currentPosition.z);

        // Xử lý xoay nghiêng model dựa trên vận tốc ngang
        if (rotationModel != null)
        {
            // Nếu vận tốc đủ lớn => nghiêng theo tỉ lệ
            if (Mathf.Abs(velocityX) >= rotationVelocityThreshold)
            {
                // Xoay tỉ lệ với vận tốc, clamp không quá góc tối đa
                targetYRotation = Mathf.Clamp(velocityX * rotationAngle, -rotationAngle, rotationAngle);
            }
            else
            {
                // Vận tốc nhỏ => về vị trí thẳng
                targetYRotation = 0f;
            }

            float newYRotation = Mathf.SmoothDampAngle(
                rotationModel.transform.localEulerAngles.y,
                targetYRotation,
                ref currentYVelocity,
                rotationSmoothTime
            );

            rotationModel.transform.localRotation = Quaternion.Euler(0f, newYRotation, 0f);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("obstacle") && !isGameOver)
        {
            failSongPlayer.Play();
            isGameOver = true;
            Debug.Log("Game Over!");
        }
    }
}
