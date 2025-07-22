using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    public void SelectMale()
    {
        PlayerManager.Instance.SelectCharacter(0); // 0 = nam
    }

    public void SelectFemale()
    {
        PlayerManager.Instance.SelectCharacter(1); // 1 = nữ
    }

    public GameObject[] characters; // Kéo các Prefab model 3D vào đây
    private int currentIndex = 0;

    void Start()
    {
        ShowCharacter(currentIndex);
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characters.Length) currentIndex = 0;
        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = characters.Length - 1;
        ShowCharacter(currentIndex);
    }

    void ShowCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            bool isActive = (i == index);
            characters[i].SetActive(isActive);
            if (isActive)
            {
                CharacterFader fader = characters[i].GetComponent<CharacterFader>();
                if (fader != null)
                    fader.FadeIn(0.3f); // fade in 0.3s
            }
        }

        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        Debug.Log("Loaded character index: " + currentIndex);
    }

}