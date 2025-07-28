using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueUI : MonoBehaviour
{
    [SerializeField] private Image countdownCircle;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Button restartButton;
    [SerializeField] AudioSource pauseSong;
    GameObject mainAudio;
    [SerializeField] PlayerInfo playerinfo;
    private float countdownTime = 5f;
    private float currentTime;

    private void OnEnable()
    {
        currentTime = countdownTime;
        restartButton.onClick.AddListener(OnContinue);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(OnContinue);
    }
    private void Start()
    {
        
        GameObject mainAudio = GameObject.Find("Audio");
        if (mainAudio != null)
            pauseSong = mainAudio.GetComponent<AudioSource>();
        else
            Debug.LogWarning("Không tìm thấy ScoreText trong scene!");
    }
    private void Update()
    {
        currentTime -= Time.unscaledDeltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndCountdown();
        }

        // Update UI
        countdownCircle.fillAmount = currentTime / countdownTime;
        countdownText.text = Mathf.CeilToInt(currentTime).ToString();
    }

    private void OnContinue()
    {
        // Xử lý tiếp tục game
        Debug.Log("Người chơi chọn tiếp tục");
        PlayerInfo.isGameOver = false;
        // Lấy tên scene hiện tại
        string currentScene = SceneManager.GetActiveScene().name;

        // Load lại scene (Single => xoá sạch mọi thứ khác, giống lần đầu load)
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        Time.timeScale = 1f;// thời gian bình thường

        //code reload scene
        //Scene curentScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(curentScene.name);
        if (SongState.SongStarted == true)
        {
            pauseSong.Play();
        }
        
        gameObject.SetActive(false);// tắt ui continue
    }
    private void EndCountdown()
    {
        // Hết giờ tự xử lý (ví dụ về menu chính hoặc restart)
        Debug.Log("Hết thời gian, không tiếp tục");
        gameObject.SetActive(false);// tắt ui continue
        SceneManager.LoadSceneAsync("0_menuTest");
        PlayerInfo.isGameOver = false;
        Time.timeScale = 1f;
    }
}
