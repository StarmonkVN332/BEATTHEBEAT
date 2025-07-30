using UnityEngine;

public class FrameManager : MonoBehaviour
{
    [Header("Frame Settings")]
    [Range(30, 60)]
    public int MaxRate = 60;

    private float targetFrameTime;

    void Awake()
    {
        // Disable VSync for manual control
        QualitySettings.vSyncCount = 0;
        // Set target frame rate, capped for mobile
        Application.targetFrameRate = MaxRate;
        targetFrameTime = 1.0f / MaxRate;
    }

    void Update()
    {
        // Ensure the game maintains the target frame time
        if (Time.unscaledDeltaTime > targetFrameTime)
        {
            // Optional: Log warning if frame time exceeds target (for debugging)
            // Debug.LogWarning("Frame time exceeded target: " + Time.unscaledDeltaTime);
        }
    }
}