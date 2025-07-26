using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUi : MonoBehaviour
{
    [SerializeField] private GameObject panelUI;
    [SerializeField] AudioSource pauseSong;

    public void PanelUITrue()
    {
        panelUI.SetActive(true);
        Time.timeScale = 0f;
        if (SongState.SongStarted == true)
        {
            pauseSong.Pause();
        }
        
    }

    public void PanelUIFalse()
    {
        panelUI.SetActive(false);
        Time.timeScale = 1f;
        if (SongState.SongStarted == true)
        {
            pauseSong.Play();
        }

    }

    public void BackHomeMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ReloadCurrentScene()
    {
        // Lấy tên scene hiện tại
        string currentScene = SceneManager.GetActiveScene().name;

        // Load lại scene (Single => xoá sạch mọi thứ khác, giống lần đầu load)
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
