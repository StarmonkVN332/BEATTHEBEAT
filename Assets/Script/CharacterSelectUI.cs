using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("Character Models")]
    public GameObject[] characters; // Kéo prefab nhân vật vào đây
    private int currentIndex = 0;

    void Awake()
    {
        // Tắt tất cả nhân vật trước khi hiển thị
        foreach (var character in characters)
        {
            if (character != null)
                character.SetActive(false);
        }

        // Load lại index từ PlayerManager để hiển thị mặc định
        currentIndex = PlayerManager.Instance.selectedIndex;
    }

    void Start()
    {
        ShowCharacter(currentIndex);
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characters.Length)
            currentIndex = 0;

        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = characters.Length - 1;

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
                    fader.FadeIn(0.3f);
            }
        }

        // ❌ KHÔNG gọi SelectCharacter ở đây
        Debug.Log("Xem trước nhân vật index: " + index);
    }

    // Nhấn nút tròn trắng để xác nhận chọn
    public void ConfirmSelection()
    {
        PlayerManager.Instance.SelectCharacter(currentIndex); // Ghi nhận lựa chọn thật sự
        Debug.Log("Đã xác nhận chọn nhân vật index: " + currentIndex);
    }
}
