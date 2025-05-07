using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("player info")]
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private float invincibleDuration = 2f;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    
    private bool isInvincible = false;
    private Renderer[] renderers;
    private Color[] originalColors;
   


    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        UpdatePlayerInfo();
    }
    public void PlayerTakeDamage(int amount)
    {
        if (isInvincible) return;

        playerHealth -= amount;
        if (playerHealth < 0) playerHealth = 0;

        UpdatePlayerInfo();
        //them aniamtion getHit tai day...
        StartCoroutine(InvincibilityFlash());
        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;
        bool flash = false;

        while (elapsed < invincibleDuration)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = flash ? Color.red : originalColors[i];
            }

            flash = !flash;
            elapsed += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }

        // Khôi phục màu gốc
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

        isInvincible = false;
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
