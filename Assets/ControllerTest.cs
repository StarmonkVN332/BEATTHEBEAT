using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ControllerTest : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float travelSpeed = 5f; // tốc độ chạy theo spline

    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private float lateralMoveRange = 2f; // giới hạn kéo trái/phải
    [SerializeField] private float lateralSensitivity = 5f; // độ nhạy khi kéo

    public NotePool notePool;

    private float distanceAlongSpline = 0f;
    private float lateralOffset = 0f; // vị trí kéo ngang hiện tại
    private Vector2 previousTouchPos;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver || splineContainer == null) return;

        // Di chuyển dọc spline
        distanceAlongSpline += travelSpeed * Time.deltaTime;
        float totalLength = splineContainer.CalculateLength();
        float t = distanceAlongSpline / totalLength;

        if (t > 1f) t = 1f; // Chạm cuối spline

        // Lấy vị trí và hướng từ spline
        splineContainer.Evaluate(t, out var posF3, out var tangentF3, out _);

        Vector3 pos = (Vector3)posF3;
        Vector3 tangent = (Vector3)tangentF3;

        Vector3 offset = Vector3.Cross(tangent, Vector3.up).normalized * lateralOffset;

        transform.position = pos + offset;
        transform.rotation = Quaternion.LookRotation(tangent);

        // Kéo trái/phải theo cảm ứng (mượt và đúng chiều)
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

                float targetOffset = lateralOffset - (delta.x / Screen.width) * lateralSensitivity; // đảo ngược nếu cần
                lateralOffset = Mathf.Lerp(lateralOffset, targetOffset, 0.25f); // làm mượt
                lateralOffset = Mathf.Clamp(lateralOffset, -lateralMoveRange, lateralMoveRange); // giới hạn

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
        if (note != null && notePool != null)
        {
            notePool.ReturnNote(note.gameObject);
            Debug.Log("Trả enemy về pool thành công");
        }

        if (!isGameOver && other.CompareTag("obstacle"))
        {
            PlayerInfo playerInfo = other.GetComponent<PlayerInfo>();
            playerInfo.PlayerTakeDamage(1);
        }
    }
}
