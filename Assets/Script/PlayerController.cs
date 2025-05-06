using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;


    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = touch.position;
                float screenWidth = Screen.width;

                if (touchPos.x < screenWidth / 3f)
                {
                    transform.position = point1.position; // chạm bên trái
                }
                else if (touchPos.x < 2f * screenWidth / 3f)
                {
                    transform.position = point2.position; // chạm giữa
                }
                else
                {
                    transform.position = point3.position; // chạm bên phải
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            animator.SetBool("ATK",true);
            StartCoroutine(ResetAttack());
        }
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.3f); // Thời gian attack animation
        animator.SetBool("ATK", false);
    }

}
