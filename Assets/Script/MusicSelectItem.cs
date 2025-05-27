using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSelectItem : MonoBehaviour
{
   
[SerializeField] private string gameplaySceneName ="Test"; // tên scene gameplay
 public void OnSelectThisTrack()
        {
            // Tùy chọn: lưu tên bài nhạc để dùng trong gameplay scene
            PlayerPrefs.SetString("SelectedTrack", gameplaySceneName);

            // Load scene chơi nhạc
            SceneManager.LoadScene(gameplaySceneName);
        }
    }
