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
    [SerializeField] private Button continueButton;

    private float countdownTime = 5f;
    private float currentTime;

    private void OnEnable()
    {
        currentTime = countdownTime;
        continueButton.onClick.AddListener(OnContinue);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(OnContinue);
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
        PlayerController.isGameOver = false;
        Time.timeScale = 1f;
        Scene curentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curentScene.name);
        gameObject.SetActive(false);
    }

    private void EndCountdown()
    {
        // Hết giờ tự xử lý (ví dụ về menu chính hoặc restart)
        Debug.Log("Hết thời gian, không tiếp tục");
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(0);
    }
}
