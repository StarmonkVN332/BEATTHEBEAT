using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject malePrefab;
    public GameObject femalePrefab;

    public int selectedIndex = 0;

    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/character.json";

            LoadData(); // Tải nhân vật đã chọn khi game bắt đầu
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(int index)
    {
        selectedIndex = index;
        SaveData();
    }

    public GameObject GetSelectedPrefab()
    {
        return selectedIndex == 0 ? malePrefab : femalePrefab;
    }

    public void SaveData()
    {
        CharacterData data = new CharacterData();
        data.selectedIndex = selectedIndex;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("Saved character to: " + savePath);
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);
            selectedIndex = data.selectedIndex;
            Debug.Log("Loaded character index: " + selectedIndex);
        }
        else
        {
            Debug.Log("No save file found. Using default character.");
        }
    }
}
