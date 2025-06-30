using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject continueUI;
    [SerializeField] private AudioSource pauseSong;

    private void Update()
    {
        if (PlayerController.isGameOver)
        {
            pauseSong.Pause();
            continueUI.SetActive(true);
            Debug.Log("continue Ui setActive true");

            Time.timeScale = 0f; // Dừng game
        }
    }
}
