using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private TextMeshProUGUI hpText;

    private void Start()
    {
        UpdateHPDisplay();
    }
    public void TakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth < 0) playerHealth = 0;

        UpdateHPDisplay();


        if (playerHealth <= 0)
        {
            Die();
        }
    }
    void UpdateHPDisplay()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + playerHealth.ToString();
        }
    }
    void Die()
    {
        Debug.Log("Player Die!");
    }
}
