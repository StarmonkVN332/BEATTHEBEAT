using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private float moveSpeed = 5f; // tốc độ di chuyển ngang
    [SerializeField] private float maxOffsetX = 2.5f; // giới hạn trái/phải theo trục X

    [Header("Spline Settings")]
    [SerializeField] private SplineAnimate splineAnimate;

    public NotePool notePool;

    private Vector2 previousTouchPos;
    private float currentX = 0f; // vị trí ngang hiện tại
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // Di chuyển dọc spline theo Z (forward)
        splineAnimate.NormalizedTime += forwardSpeed * Time.deltaTime;

        float t = splineAnimate.NormalizedTime;

        SplineUtility.Evaluate(splineAnimate.Container.Spline, t, out float3 splinePosF3, out _, out _);
        Vector3 splinePosition = (Vector3)splinePosF3;

        // Xử lý kéo để di chuyển theo X
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
                currentX += deltaX;

                currentX = Mathf.Clamp(currentX, -maxOffsetX, maxOffsetX);

                if (animator != null)
                    animator.SetTrigger("isMoving");
            }
        }

        // Gán vị trí mới: X lấy từ currentX, Z từ spline
        transform.position = new Vector3(currentX, splinePosition.y, splinePosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AnimationTrigger"))
        {
            animator.SetTrigger("isAttackRight");
            slashSound.Play();
            slashEffect.Play();
        }

        Enemy note = other.GetComponent<Enemy>();
        if (note != null && notePool != null)
        {
            notePool.ReturnNote(note.gameObject);
            Debug.Log("Trả enemy về pool thành công");
        }

        if (!isGameOver && other.CompareTag("obstacle"))
        {
            PlayerInfo playerInfo = other.GetComponent<PlayerInfo>();
            playerInfo.PlayerTakeDamage(1);
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
