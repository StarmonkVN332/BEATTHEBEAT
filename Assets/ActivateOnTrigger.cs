using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Firebase.Auth;
public class ActivateOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject winPannel; // Kéo thả object vào đây trong Inspector
    [SerializeField] private TextMeshProUGUI summaryScore; 
    private PlayerInfo playerInfo;
    private FirebaseManager firebaseManager;

    private void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("❌ Không tìm thấy FirebaseManager!");
            return; // Dừng lại để không gọi LoadUserData khi firebaseManager null
        }
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        // Cách 1: Tìm trực tiếp trong scene
        playerInfo = FindObjectOfType<PlayerInfo>();

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

                firebaseManager.SaveScoreFromGame(playerInfo.score); // ✅ Lưu điểm tổng
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
