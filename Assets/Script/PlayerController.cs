using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [Header("Spline")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float forwardSpeed = 0.2f; // tốc độ chạy dọc spline (0 - 1)

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxOffsetX = 2f;

    [Header("Visuals & Effects")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioSource slashSound;

    private float normalizedT = 0f;
    private float xOffset = 0f;
    private Vector2 previousTouchPos;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // Di chuyển dọc theo spline
        normalizedT += forwardSpeed * Time.deltaTime;
        normalizedT = Mathf.Clamp01(normalizedT); // giữ trong 0 - 1

        splineContainer.Spline.Evaluate(normalizedT, out float3 splinePosF3, out float3 splineTangent, out _);
        Vector3 splinePos = (Vector3)splinePosF3;

        // Xử lý điều khiển kéo trái/phải
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
                xOffset = Mathf.Clamp(xOffset + deltaX, -maxOffsetX, maxOffsetX);

                if (animator != null)
                    animator.SetTrigger("isMoving");
            }
        }

        // Gán vị trí mới: spline + xOffset
        Vector3 targetPos = splinePos + transform.right * xOffset;
        transform.position = targetPos;
        transform.rotation = Quaternion.LookRotation((Vector3)splineTangent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AnimationTrigger"))
        {
            animator.SetTrigger("isAttackRight");
            slashSound?.Play();
            slashEffect?.Play();
        }

        if (!isGameOver && other.CompareTag("obstacle"))
        {
            PlayerInfo playerInfo = other.GetComponent<PlayerInfo>();
            playerInfo?.PlayerTakeDamage(1);
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }
}
