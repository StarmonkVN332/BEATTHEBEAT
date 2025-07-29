using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] musicClips;

    private List<AudioClip> playlist = new List<AudioClip>();
    private int currentClipIndex = 0;

    private void Start()
    {
        ShufflePlaylist();
        PlayNextMusic();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    private void ShufflePlaylist()
    {
        playlist = new List<AudioClip>(musicClips);
        for (int i = 0; i < playlist.Count; i++)
        {
            int randIndex = Random.Range(i, playlist.Count);
            (playlist[i], playlist[randIndex]) = (playlist[randIndex], playlist[i]); // Hoán đổi
        }

        currentClipIndex = 0;
    }

    private void PlayNextMusic()
    {
        if (playlist.Count == 0 || audioSource == null)
        {
            Debug.LogWarning("Không có nhạc hoặc AudioSource chưa gán!");
            return;
        }

        if (currentClipIndex >= playlist.Count)
        {
            ShufflePlaylist(); // Khi phát hết playlist, xáo lại
        }

        audioSource.clip = playlist[currentClipIndex];
        audioSource.Play();
        currentClipIndex++;
    }
}
