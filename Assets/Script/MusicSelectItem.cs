using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSelectItem : MonoBehaviour
{
    public string gameplaySceneName = "Test";
    public AudioClip clickSound;
    public AudioSource uiAudioSource;
    [SerializeField] private MainSceneManager mainSceneManager;

    public void OnSelectThisTrack()
    {
        // Phát âm thanh click
        if (clickSound != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
            // Đợi âm xong rồi mới chuyển scene
            Invoke(nameof(LoadScene), clickSound.length);
        }
        else
        {
            // Không có âm thanh thì load liền
            LoadScene();
        }
    }

    void LoadScene()
    {
        mainSceneManager.ChangeScene(gameplaySceneName);
    }
}