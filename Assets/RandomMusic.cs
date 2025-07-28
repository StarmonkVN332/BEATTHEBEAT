using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;   // Kéo AudioSource của bạn vào đây
    [SerializeField] private AudioClip[] musicClips;   // Thêm các clip nhạc vào đây trong Inspector

    private void Start()
    {
        PlayRandomMusic();
    }

    private void PlayRandomMusic()
    {
        if (musicClips.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Chưa gán AudioSource hoặc chưa có nhạc nào trong list!");
            return;
        }

        int randomIndex = Random.Range(0, musicClips.Length);
        audioSource.clip = musicClips[randomIndex];
        audioSource.Play();
    }
}
