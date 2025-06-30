using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("Nhét nhạc chính của scene vào pauseSong")]
    [SerializeField] private GameObject continueUI;
    [SerializeField] private AudioSource pauseSong;

    private void Update()
    {
        if (PlayerController.isGameOver)
        {
            pauseSong.Pause();// chỗ này nhét nhạc chính của scene đó
            continueUI.SetActive(true);// chỗ này chỉ nhét ui continue
            Debug.Log("continue Ui setActive true");

            Time.timeScale = 0f; // Dừng game
        }
    }
}
