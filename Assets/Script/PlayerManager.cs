using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject malePrefab;
    public GameObject femalePrefab;

    private int selectedIndex = 0; // 0 = male, 1 = female

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(int index)
    {
        selectedIndex = index;
    }

    public GameObject GetSelectedPrefab()
    {
        return selectedIndex == 0 ? malePrefab : femalePrefab;
    }
}