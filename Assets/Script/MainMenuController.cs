using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMusicSelection;
    public GameObject panelMainMenu; // <- thêm biến này

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
}
