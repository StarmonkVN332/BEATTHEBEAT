using System.Collections.Generic;
using UnityEngine;

public class NoteS : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoint;
    public GameObject notePrefab; // Prefab của note
    public AudioSource musicSource; // Nhạc nền

    [SerializeField]private List<float> noteTimestamps = new List<float>() { 1f, 3f, 10f, 11.5f };
    private int currentNoteIndex = 0;

    void Update()
    {
        if (currentNoteIndex < noteTimestamps.Count &&
            musicSource.time >= noteTimestamps[currentNoteIndex])
        {
            SpawnNote();
            currentNoteIndex++;
        }
    }

    void SpawnNote()
    {
        Debug.Log("Bat dau spawn enemy");
        if (spawnPoint.Count == 0) return;

        // Chọn ngẫu nhiên 1 vị trí spawn
        int randomIndex = Random.Range(0, spawnPoint.Count);
        Transform spawnPos = spawnPoint[randomIndex];

        // Spawn tại vị trí đó
        Instantiate(notePrefab, spawnPos.position, Quaternion.identity);
    }
}
