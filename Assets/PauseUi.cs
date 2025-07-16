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
}
