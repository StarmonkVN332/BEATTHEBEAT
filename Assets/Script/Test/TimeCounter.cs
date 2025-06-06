using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text timeText; // Assign in Inspector
    private float elapsedTime = 0f;
    private bool isCounting = true;

    void Update()
    {
        if (isCounting)
        {
            elapsedTime += Time.deltaTime;

            int hours = Mathf.FloorToInt(elapsedTime / 3600);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

            timeText.text = $"{hours:00}:{minutes:00}:{seconds:00}.{milliseconds:000}";
        }
    }

    public void StartCounter() => isCounting = true;
    public void StopCounter() => isCounting = false;

    public void ResetCounter()
    {
        elapsedTime = 0f;
        timeText.text = "00:00:00.000";
    }
}
