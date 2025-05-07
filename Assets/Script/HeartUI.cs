using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUI : MonoBehaviour
{
    public List<Image> heartFullImages; // Gán từng Image component (trái tim đỏ)

    public int maxHearts = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHearts;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHearts);
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartFullImages.Count; i++)
        {
            heartFullImages[i].enabled = i < currentHealth;
        }
    }
}
