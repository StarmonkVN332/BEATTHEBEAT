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

    // Nhảy & trọng lực tự tạo
    private float verticalVelocity = 0f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float gravity = -35f;

    void Start()
    {
        targetX = transform.position.x;
        if (collider != null)
            collider.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (!GameState.GameStarted || isGameOver)
            return;

        Vector3 currentPosition = transform.position;
        verticalVelocity += gravity * Time.deltaTime;

        //─────────────────────────────────────────────────────────────
        // TOUCH TRÊN THIẾT BỊ DI ĐỘNG
        //─────────────────────────────────────────────────────────────
        if (Input.touchCount > 0)
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
                Debug.Log($"[TOUCH] deltaY = {deltaY}, isGrounded = {isGrounded}");

                if (deltaY > 0.03f && isGrounded)
                {
                    verticalVelocity = jumpForce;
                    isGrounded = false;
                    animator.SetTrigger("Jump");
                    Debug.Log("Jumped via TOUCH, verticalVelocity = " + verticalVelocity);
                }
            }
        }

#if UNITY_EDITOR
        //─────────────────────────────────────────────────────────────
        // GIẢ LẬP TRONG EDITOR BẰNG CHUỘT
        //─────────────────────────────────────────────────────────────
        if (Input.GetMouseButtonDown(0))
        {
            previousTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - previousTouchPos;
            previousTouchPos = Input.mousePosition;

            float deltaX = (delta.x / Screen.width) * moveSpeed;
            targetX = Mathf.Clamp(targetX + deltaX, minX, maxX);
            animator.SetTrigger("isMoving");

            float deltaY = delta.y / Screen.height;
            Debug.Log($"[MOUSE] deltaY = {deltaY}, isGrounded = {isGrounded}");

            if (deltaY > 0.03f && isGrounded)
            {
                verticalVelocity = jumpForce;
                isGrounded = false;
                animator.SetTrigger("Jump");
                Debug.Log("Jumped via MOUSE, verticalVelocity = " + verticalVelocity);
            }
        }
#endif

        //─────────────────────────────────────────────────────────────
        // VẬT LÝ TỰ TẠO
        //─────────────────────────────────────────────────────────────
        currentPosition.x = Mathf.SmoothDamp(currentPosition.x, targetX, ref velocityX, smoothTime);
        currentPosition.y += verticalVelocity * Time.deltaTime;

        // Xử lý chạm đất
        if (currentPosition.y <= 0.01f && verticalVelocity <= 0f)
        {
            currentPosition.y = 0f;
            verticalVelocity = 0f;
            if (!isGrounded)
            {
                isGrounded = true;
                Debug.Log("Landed");
            }
        }

        currentPosition.z += forwardSpeed * Time.deltaTime;
        transform.position = currentPosition;

        //─────────────────────────────────────────────────────────────
        // XOAY MODEL THEO DI CHUYỂN
        //─────────────────────────────────────────────────────────────
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

    //─────────────────────────────────────────────────────────────
    // XỬ LÝ GAME OVER & CHẠM VẬT CẢN
    //─────────────────────────────────────────────────────────────
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
