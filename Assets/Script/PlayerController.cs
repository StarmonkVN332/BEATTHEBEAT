using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;
    [SerializeField] private float smoothTime = 0.1f;
    public bool isGrounded = false;

    [SerializeField] private Collider collider;
    [SerializeField] private GameObject Continue_UI;
    [SerializeField] private AudioSource failSongPlayer;
    public static bool isGameOver = false;

    [Header("Model Rotation")]
    public GameObject rotationModel;
    [SerializeField] private float rotationAngle = 20f;
    [SerializeField] private float rotationSmoothTime = 0.2f;
    [SerializeField] private float rotationVelocityThreshold = 0.1f;

    private float targetX;
    private float velocityX = 0f;

    private Vector2 previousTouchPos;
    private float targetYRotation = 0f;
    private float currentYVelocity = 0f;

    // Nhảy & trọng lực
    private float verticalVelocity = 0f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravity = -25f;

    void Start()
    {
        targetX = transform.position.x;
        collider.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (!GameState.GameStarted || isGameOver)
            return;

        Vector3 currentPosition = transform.position;

        // Thêm trọng lực
        verticalVelocity += gravity * Time.deltaTime;

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

                float deltaY = delta.y / Screen.height;
                if (deltaY > 0.1f && isGrounded)
                {
                    verticalVelocity = jumpForce;
                    isGrounded = false;
                    animator.SetTrigger("Jump");
                    Debug.Log("Jumped! deltaY: " + deltaY);
                }
            }
        }

        // Làm mượt trục X
        currentPosition.x = Mathf.SmoothDamp(currentPosition.x, targetX, ref velocityX, smoothTime);
        // Áp dụng vận tốc Y
        currentPosition.y += verticalVelocity * Time.deltaTime;

        // Chặn rơi xuống sàn
        if (currentPosition.y <= 0f)
        {
            currentPosition.y = 0f;
            verticalVelocity = 0f;
            isGrounded = true;
        }

        // Di chuyển tới trước
        currentPosition.z += forwardSpeed * Time.deltaTime;
        transform.position = currentPosition;

        // Xử lý xoay model
        if (rotationModel != null)
        {
            if (Mathf.Abs(velocityX) >= rotationVelocityThreshold)
            {
                targetYRotation = Mathf.Clamp(velocityX * rotationAngle, -rotationAngle, rotationAngle);
            }
            else
            {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle") && !isGameOver)
        {
            failSongPlayer.Play();
            isGameOver = true;
            Debug.Log("Game Over!");
            if (Continue_UI != null)
                Continue_UI.SetActive(true);
        }

        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
