using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    [Header("player info")]
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private int combo = 0;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI comboText;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RandomSoundPlayer randomSoundPlayer;
    [SerializeField] private ParticleSystem boomEffect;

    private int basePointPerNote = 100;

    private void Start()
    {
        // Tìm text theo tên (nếu có trong scene)
        GameObject scoreObj = GameObject.Find("ScoreText");
        GameObject comboObj = GameObject.Find("ComboText");

        if (scoreObj != null)
            scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        else
            Debug.LogWarning("Không tìm thấy ScoreText trong scene!");

        if (comboObj != null)
            comboText = comboObj.GetComponent<TextMeshProUGUI>();
        else
            Debug.LogWarning("Không tìm thấy ComboText trong scene!");

        UpdatePlayerInfo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HandleNoteHit();
            Debug.Log("Chạm vào Enemy: Ăn note thành công");
            randomSoundPlayer.PlayRandomSound();
            boomEffect.Play();
            Destroy(other.gameObject);
        }
    }

    public void PlayerTakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth < 0) playerHealth = 0;

        HandleNoteMiss(); // Mất máu = miss

        UpdatePlayerInfo();

        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void HandleNoteHit()
    {
        float multiplier = 1f;

        if (combo > 20)
            multiplier = 2f;
        else if (combo > 15)
            multiplier = 1.5f;
        else if (combo > 10)
            multiplier = 1.25f;

        int pointGained = Mathf.RoundToInt(basePointPerNote * multiplier);
        score += pointGained;
        combo++;

        Debug.Log($"Hit! Combo: {combo}, Score Gained: {pointGained}, Total Score: {score}");

        UpdatePlayerInfo();
    }

    private void HandleNoteMiss()
    {
        Debug.Log($"Miss! Combo Reset. Previous Combo: {combo}");
        combo = 0;
        UpdatePlayerInfo();
    }

    private void UpdatePlayerInfo()
    {
        if (scoreText != null)
        {
            scoreText.text = " " + score.ToString();
        }

        if (comboText != null)
        {
            comboText.text = "x" + combo.ToString();
        }
    }

    private void PlayerDie()
    {
        Debug.Log("Player Die!");
    }

    public void SendHitNote()
    {
        HandleNoteHit();
    }

    public void SendMissNote()
    {
        Debug.Log("Miss Note! Combo reset.");
        HandleNoteMiss();
    }
}
