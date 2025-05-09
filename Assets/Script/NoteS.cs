using System.Collections.Generic;
using UnityEngine;

public class NoteS : MonoBehaviour
{
    public GameObject notePrefab;
    public AudioSource musicSource;
    public Transform[] laneSpawnPoints; // 0 = left, 1 = middle, 2 = right
    public TextAsset beatmapJson;

    [SerializeField] private float delayMusicStart = 3f;

    private List<NoteData> noteList;
    private int currentNoteIndex = 0;
    private float startTime;

    void Start()
    {
        // Load beatmap
        Beatmap beatmap = JsonUtility.FromJson<Beatmap>(beatmapJson.text);
        noteList = beatmap.notes;

        startTime = Time.time;
        StartCoroutine(StartMusicWithDelay());

        Debug.Log($"Loaded {noteList.Count} notes from beatmap.");
    }

    System.Collections.IEnumerator StartMusicWithDelay()
    {
        yield return new WaitForSeconds(delayMusicStart);
        musicSource.Play();
        Debug.Log($"Music started at time: {Time.time - startTime:F2}s");
    }

    void Update()
    {
        if (currentNoteIndex >= noteList.Count) return;

        float elapsedTime = Time.time - startTime;
        NoteData nextNote = noteList[currentNoteIndex];

        if (elapsedTime >= nextNote.time)
        {
            Debug.Log($"Spawning Note {currentNoteIndex + 1}/{noteList.Count} | Time: {elapsedTime:F2}s | Lane: {nextNote.lane}");
            SpawnNote(nextNote.lane);
            currentNoteIndex++;
        }
    }

    void SpawnNote(int lane)
    {
        if (lane < 0 || lane >= laneSpawnPoints.Length)
        {
            Debug.LogWarning($"Invalid lane index: {lane}");
            return;
        }

        Transform spawnPos = laneSpawnPoints[lane];
        GameObject note = Instantiate(notePrefab, spawnPos.position, Quaternion.identity);

        // Rename the note GameObject to include timing and lane info
        float noteTime = noteList[currentNoteIndex].time;
        note.name = $"Note_{currentNoteIndex + 1}_Time{noteTime:F2}_Lane{lane}";

        Debug.Log($"Spawned: {note.name}");
    }

}

[System.Serializable]
public class NoteData
{
    public float time;
    public int lane;
}

[System.Serializable]
public class Beatmap
{
    public List<NoteData> notes;
}
