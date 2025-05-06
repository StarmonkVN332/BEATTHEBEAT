using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    //[SerializeField] private Transform point1;
    //[SerializeField] private Transform point2;
    //[SerializeField] private Transform point3;

    [SerializeField] private Collider leftZone;
    [SerializeField] private Collider midZone;
    [SerializeField] private Collider rightZone;


    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform midPos;
    [SerializeField] private Transform rightPos;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                float screenWidth = Screen.width;
                Vector2 touchPos = touch.position;

                if (touchPos.x < screenWidth / 3f)
                {
                    transform.position = leftPos.position;
                    TryAttack(leftZone);
                }
                else if (touchPos.x < 2f * screenWidth / 3f)
                {
                    transform.position = midPos.position;
                    TryAttack(midZone);
                }
                else
                {
                    transform.position = rightPos.position;
                    TryAttack(rightZone);
                }
            }
        }
    }


    void TryAttack(Collider attackZone)
    {
        Collider[] hits = Physics.OverlapBox(attackZone.bounds.center, attackZone.bounds.extents, attackZone.transform.rotation);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>().TakeDamage(1); // hoặc gọi hàm tấn công khác
                Debug.Log("Tấn công enemy trong vùng!");
                return;
            }
        }

        Debug.Log("Không có enemy trong vùng này.");
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        animator.SetBool("ATK",true);
    //        StartCoroutine(ResetAttack());
    //    }
    //}
    //IEnumerator ResetAttack()
    //{
    //    yield return new WaitForSeconds(0.3f); // Thời gian attack animation
    //    animator.SetBool("ATK", false);
    //}

}
