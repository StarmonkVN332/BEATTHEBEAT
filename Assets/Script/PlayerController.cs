using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("CheckBoxx")]
    [SerializeField] private Collider leftZone;
    [SerializeField] private Collider midZone;
    [SerializeField] private Collider rightZone;

    [Header("Positon Player Transform")]
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
                    animator.SetTrigger("isAttackLeft");
                }
                else if (touchPos.x < 2f * screenWidth / 3f)
                {
                    transform.position = midPos.position;
                    TryAttack(midZone);
                    animator.SetTrigger("isAttackMiddle");
                }
                else
                {
                    transform.position = rightPos.position;
                    TryAttack(rightZone);
                    animator.SetTrigger("isAttackRight");
                }
            }
        }
    }


    private void TryAttack(Collider attackZone)
    {
        Collider[] hits = Physics.OverlapBox(attackZone.bounds.center, attackZone.bounds.extents, attackZone.transform.rotation);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>().EnemyTakeDamage(1);

                //Debug.Log("Tấn công enemy trong vùng!");
                return;
            }
        }

        //Debug.Log("Không có enemy trong vùng này.");
    }
}
