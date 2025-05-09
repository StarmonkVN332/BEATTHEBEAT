using UnityEngine;

public class DeplaySong : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayInSeconds = 4f;

    void Start()
    {
        double startTime = AudioSettings.dspTime + delayInSeconds;
        audioSource.PlayScheduled(startTime);
    }
}
