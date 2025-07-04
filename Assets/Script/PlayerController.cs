using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    private Vector2 previousTouchPos;
    private float targetX;                               // vị trí x mục tiêu
    private float velocityX = 0f;                         // tốc độ tạm để smooth

    void Start()
    {
        targetX = transform.position.x;
        collider.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 🚫 Wait until notes are spawned and music started
        if (!GameState.GameStarted)
            return;

        Vector3 currentPosition = transform.position;

        // Forward movement
        currentPosition.z += forwardSpeed * Time.deltaTime;

        // Touch input
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

        // Smooth horizontal movement
        currentPosition.x = Mathf.SmoothDamp(currentPosition.x, targetX, ref velocityX, smoothTime);

        // Apply new position
        transform.position = new Vector3(currentPosition.x, 0f, currentPosition.z);
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
