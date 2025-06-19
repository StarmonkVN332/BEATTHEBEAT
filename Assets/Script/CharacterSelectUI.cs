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
}