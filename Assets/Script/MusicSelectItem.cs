using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicSelectItem : MonoBehaviour
{
    public string gameplaySceneName = "Test";
    public AudioClip clickSound;
    public AudioSource uiAudioSource;

    private MainSceneManager mainSceneManager;

    private void Awake()
    {
        // Auto-find MainSceneManager (singleton)
        mainSceneManager = MainSceneManager.Instance;

        if (mainSceneManager == null)
        {
            Debug.LogError("❌ MainSceneManager not found in the scene!");
        }
    }
    private void OnDestroy()
    {
        Debug.LogWarning("🔥 MusicSelectItem was destroyed!");
    }

    public void OnSelectThisTrack()
    {
        if (clickSound != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
            Debug.Log($"🎵 Playing click sound: {clickSound.name}");
            Debug.Log($"⏳ Waiting {clickSound.length} seconds before loading scene: {gameplaySceneName}");
            // Offload the delay and scene change to MainSceneManager (safe from destruction)
            MainSceneManager.Instance?.ChangeSceneWithClickDelay(gameplaySceneName, clickSound.length);
        }
        else
        {
            Debug.LogWarning("⚠️ No click sound or audio source assigned. Loading scene immediately.");
            MainSceneManager.Instance?.ChangeScene(gameplaySceneName);
        }
    }



    //private IEnumerator LoadSceneAfterDelay(float delay)
    //{
    //    if (string.IsNullOrEmpty(gameplaySceneName))
    //    {
    //        Debug.LogError("❌ Gameplay scene name is null or empty!");
    //        yield break;
    //    }

    //    yield return new WaitForSeconds(delay);

    //    if (MainSceneManager.Instance == null)
    //    {
    //        Debug.LogError("❌ MainSceneManager.Instance is null when trying to load scene!");
    //        yield break;
    //    }

    //    MainSceneManager.Instance.ChangeScene(gameplaySceneName);
    //    Debug.Log($"✅ Scene {gameplaySceneName} loaded successfully after {delay} seconds.");
    //}

}
