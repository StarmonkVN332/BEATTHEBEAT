using UnityEngine;
//using static UnityEditor.Progress;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMusicSelection;
    public GameObject panelMainMenu; // <- thêm biến này
    public GameObject panelPlayerSelection;
    [SerializeField] private GameObject optionsUI;

    private void Start()
    {
        Time.timeScale = 1f; // Đảm bảo game không bị dừng khi vào menu
    }
    public void OnPlayButtonClicked()
    {
        panelMusicSelection.SetActive(true);
        panelMainMenu.SetActive(false); // ẩn main menu
    }
    public void OnBackFromMusicSelection()
    {
        panelMusicSelection.SetActive(false);
        panelMainMenu.SetActive(true);
    }
    public void OnBackPlayerSelection()
    {
        panelPlayerSelection.SetActive(false);
    }
    public void PlayerSelection()
    {
        panelPlayerSelection.SetActive(true);
    }
    public void PannelOptionsTrue()
    {
        optionsUI.SetActive(true);
    }
    public void PannelOptionsFalse()
    {
        optionsUI.SetActive(false);
    }
}
