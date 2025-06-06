using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public float delayTime = 1.5f; // thời gian delay trước khi chuyển scene (giây)

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneWithDelay(sceneName));
    }

    private IEnumerator ChangeSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayTime); // đợi delayTime giây
        SceneManager.LoadScene(sceneName);
    }
}