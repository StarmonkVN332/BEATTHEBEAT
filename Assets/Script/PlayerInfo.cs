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

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RandomSoundPlayer randomSoundPlayer;
    [SerializeField] private ParticleSystem boomEffect;

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
            randomSoundPlayer.PlayRandomSound();
            boomEffect.Play();
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
