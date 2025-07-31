using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{
    public GameObject welcomePanel; // Panel chứa chữ Tap to Login
    public GameObject loginUI;      // UI đăng nhập bị ẩn ban đầu

    private void Start()
    {
        // Khi mới khởi động, chỉ hiện welcome
        welcomePanel.SetActive(true);
        loginUI.SetActive(false);
    }

    private void Update()
    {
        // Nếu người dùng chạm màn hình hoặc bấm chuột => ẩn Welcome, hiện Login
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            welcomePanel.SetActive(false);
            loginUI.SetActive(true);
        }
    }
}
