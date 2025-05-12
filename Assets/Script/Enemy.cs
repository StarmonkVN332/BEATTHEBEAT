using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private int enemyScore = 1;

    public void EnemyTakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy took damage. Remaining: " + health);
        
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
        // Tìm PlayerInfo
        PlayerInfo player = FindObjectOfType<PlayerInfo>();
        if (player != null)
        {
            player.Score(enemyScore);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy PlayerInfo để cộng điểm!");
        }

        Destroy(gameObject);
    }
}
