using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        UpdatePlayerInfo();
    }
    public void PlayerTakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth < 0) playerHealth = 0;

        UpdatePlayerInfo();


        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }
    public void Score(int amount)
    {
        score += amount;
        UpdatePlayerInfo();
    }
    private void UpdatePlayerInfo()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + playerHealth.ToString();
        }
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score.ToString();
        }
    }
    private void PlayerDie()
    {
        //them animation die
        Debug.Log("Player Die!");
    }
}
