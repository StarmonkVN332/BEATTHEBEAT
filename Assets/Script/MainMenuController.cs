using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMusicSelection;
    public GameObject panelMainMenu; // <- thêm biến này
    public GameObject panelPlayerSelection;

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
}
