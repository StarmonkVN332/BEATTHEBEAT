using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ActivateOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject winPannel; // Kéo thả object vào đây trong Inspector
    [SerializeField] private TextMeshProUGUI summaryScore; 
    private PlayerInfo playerInfo;

    private void Start()
    {
        // Cách 1: Tìm trực tiếp trong scene
        //playerInfo = FindObjectOfType<PlayerInfo>();

        // Cách 2: Hoặc public biến rồi kéo PlayerInfo vào Inspector
        // (public PlayerInfo playerInfo;)
        StartCoroutine(deplayCheckPlayerInfo());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu va chạm với Player
        if (other.CompareTag("Player"))
        {
            if (winPannel != null)
            {
                winPannel.SetActive(true);
               
                
                int currentScore = playerInfo.GetScore();
                Debug.Log("Điểm hiện tại: " + currentScore);
                summaryScore.text = currentScore.ToString();
            }
        }
    }
    IEnumerator deplayCheckPlayerInfo()
    {
        yield return new WaitForSeconds(6);
        playerInfo = FindObjectOfType<PlayerInfo>();
       
    }
}
