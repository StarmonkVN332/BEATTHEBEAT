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

    [SerializeField] private GameObject Continue_UI;
    [SerializeField] private AudioSource failSongPlayer;
    public static bool isGameOver = false;

    [Header("Model Rotation")]
    public GameObject rotationModel;
    [SerializeField] private float rotationAngle = 20f;
    [SerializeField] private float rotationSmoothTime = 0.2f;
    [SerializeField] private float rotationVelocityThreshold = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.3f;

    private float targetX;
    private float velocityX = 0f;

    private float velocityY = 0f; // cho nhảy & rơi

    private Vector2 previousTouchPos;
    private float targetYRotation = 0f;
    private float currentYVelocity = 0f;

    private bool isGrounded;
    private bool isSwiping = false;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        if (!GameState.GameStarted || isGameOver)
            return;

        HandleSwipeInput();
        HandleHorizontalTouch();
        ApplyGravityAndJump();
        RotateModel();
    }

    private void HandleHorizontalTouch()
    {
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
                targetX = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
                animator.SetTrigger("isMoving");
            }
        }

        // Di chuyển mượt theo chiều X
        float smoothX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTime);

        // Áp dụng vị trí mới (chưa tính gravity)
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z + forwardSpeed * Time.deltaTime);
    }

    private void HandleSwipeInput()
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

        if (swipeDistanceY > 50f && swipeDistanceY > swipeDistanceX)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocityY = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    private void ApplyGravityAndJump()
    {
        // Raycast từ gần chân
        Vector3 rayOrigin = transform.position + Vector3.down * 0.5f;
        float rayLength = groundCheckDistance;

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.green);

        bool wasGrounded = isGrounded;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, groundMask))
        {
            isGrounded = true;
            // Nếu mới chạm đất, đưa player chính xác lên mặt đất
            if (!wasGrounded)
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y + 0.5f; // 0.5f là khoảng cách từ pivot đến chân (tùy model)
                transform.position = pos;
                velocityY = 0;
            }
        }
        else
        {
            isGrounded = false;
            velocityY -= gravity * Time.deltaTime;
            transform.position += new Vector3(0, velocityY * Time.deltaTime, 0);
        }
    }

    
    private void RotateModel()
    {
        if (rotationModel == null) return;

        float vx = velocityX;
        if (Mathf.Abs(vx) >= rotationVelocityThreshold)
        {
            targetYRotation = Mathf.Clamp(vx * rotationAngle, -rotationAngle, rotationAngle);
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
    }

}
