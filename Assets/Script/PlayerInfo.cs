using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    [Header("Heart UI")]
    [SerializeField] private List<Image> heartImages; // Gán trái tim đỏ trong Editor
    [Header("player info")]
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private float invincibleDuration = 2f;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource slashHitSound;

    private void Start()
    {
        UpdatePlayerInfo();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Score(1);
            Debug.Log("Chạm vào Enemy: +1 điểm");
            slashHitSound.pitch = Random.Range(0.8f, 1.2f);
            slashHitSound.Play();
            Destroy(other.gameObject);
        }
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

        // Cập nhật hiển thị trái tim
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = i < playerHealth;
        }
    }
    private void PlayerDie()
    {
        //them animation die
        Debug.Log("Player Die!");
    }
}
