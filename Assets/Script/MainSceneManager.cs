using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }

    public float delayTime = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Debug.Log("[MainSceneManager] Initialized");
    }
    private void OnEnable()
    {
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneWithDelay(sceneName));
    }

    private IEnumerator ChangeSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log($"✅ Loading scene: {sceneName}");
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ChangeSceneWithClickDelay(string sceneName, float delay)
    {
        StartCoroutine(ChangeSceneAfterClickDelay(sceneName, delay));
    }

    private IEnumerator ChangeSceneAfterClickDelay(string sceneName, float delay)
    {
        Debug.Log($"[MainSceneManager] ⏳ Waiting {delay} seconds before changing to: {sceneName}");
        yield return new WaitForSeconds(delay);

        Debug.Log($"[MainSceneManager] 🧠 Now triggering ChangeScene for: {sceneName}");
        ChangeScene(sceneName);  // includes an additional delayTime if needed
    }
}
