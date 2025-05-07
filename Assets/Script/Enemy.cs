using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy took damage. Remaining: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(1); // hoặc gọi hàm tấn công khác
            Debug.Log("Gây sát thương cho player!");
            return;
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
