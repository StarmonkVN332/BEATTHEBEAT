using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    [Header("player info")]
    [SerializeField] private int score = 0;
    [SerializeField] private int combo = 0;
    [SerializeField] private GameObject Continue_UI;
    [SerializeField] private AudioSource failSongPlayer;
    public static bool isGameOver = false;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI comboText;
    private ComboTextAnim comboTextAnim;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private RandomSoundPlayer randomSoundPlayer;
    [SerializeField] private ParticleSystem boomEffect;

    private int basePointPerNote = 100;

    private void Start()
    {
        // Tìm GameObject có gắn ComboTextAnim trong scene
        comboTextAnim = FindObjectOfType<ComboTextAnim>();

        if (comboTextAnim == null)
        {
            Debug.LogWarning("Không tìm thấy ComboTextAnim trong scene!");
        }

        GameObject Continue_UI = GameObject.FindGameObjectWithTag("GameOverUI");
        if (Continue_UI != null)
        {
            Continue_UI.SetActive(false); // 👈 This disables the object
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameOverUI trong scene!");
        }
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

            if (comboTextAnim != null)
            {
                comboTextAnim.AnimationTextCombo();
            }
        }
        else if(other.CompareTag("obstacle") && !isGameOver)
        {
            failSongPlayer.Play();
            isGameOver = true;
            Debug.Log("Game Over!");
            if (Continue_UI != null)
                Continue_UI.SetActive(true);
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

        //Debug.Log($"Hit! Combo: {combo}, Score Gained: {pointGained}, Total Score: {score}");

        UpdatePlayerInfo();
    }

    public void HandleNoteMiss()
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
