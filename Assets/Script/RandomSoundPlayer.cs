using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;

    public void PlayRandomSound()
    {
        if (clips.Length == 0 || audioSource == null) return;

        // Random clip
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];

        // Random pitch
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        // Phát clip
        audioSource.PlayOneShot(clipToPlay);
    }
}
