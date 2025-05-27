//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float forwardSpeed = 22f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioSource slashSound;

    [SerializeField] private float moveSpeed = 10f; // Tốc độ phản hồi khi kéo
    [SerializeField] private float minX = -4.5f;
    [SerializeField] private float maxX = 4.5f;

    public NotePool notePool;

    private Vector2 previousTouchPos;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // Luôn di chuyển về phía trước (trục Z)
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        // Di chuyển ngang theo thao tác kéo
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

                float newX = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);

                if (animator != null)
                    animator.SetTrigger("isMoving");
            }
        }
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
        if (note != null)
        {
            if (notePool != null)
            {
                notePool.ReturnNote(note.gameObject);
                Debug.Log("Trả enemy về pool thành công");
            }
        }
        if (!isGameOver && other.CompareTag("obstacle"))
        {
            PlayerInfo playerInfo = other.GetComponent<PlayerInfo>();
            playerInfo.PlayerTakeDamage(1);
            //GameOver();
        }
    }
    //void GameOver()
    //{
    //    isGameOver = true;
    //    Debug.Log("Game Over!");
    //    Time.timeScale = 0f; // Dừng game
    //}
}
