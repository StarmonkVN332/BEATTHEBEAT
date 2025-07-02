using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUi : MonoBehaviour
{
    [SerializeField] private GameObject panelUI;

    public void PanelUITrue()
    {
        panelUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PanelUIFalse()
    {
        panelUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackHomeMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
