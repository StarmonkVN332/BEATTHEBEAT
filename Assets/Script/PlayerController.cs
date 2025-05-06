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
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = point1.position;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position = point2.position;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position = point3.position;
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
