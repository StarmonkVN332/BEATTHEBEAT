using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public float blinkSpeed = 0.5f; // Tốc độ chớp (giây)
    private TextMeshProUGUI text;
    private bool isVisible = true;
    private float timer;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkSpeed)
        {
            isVisible = !isVisible;
            text.alpha = isVisible ? 1f : 0f;
            timer = 0f;
        }
    }
}
