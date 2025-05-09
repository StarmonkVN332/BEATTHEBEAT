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

        // Record when gameplay started
        startTime = Time.time;

        // Start coroutine to delay music
        StartCoroutine(StartMusicWithDelay());
    }

    System.Collections.IEnumerator StartMusicWithDelay()
    {
        yield return new WaitForSeconds(delayMusicStart);
        musicSource.Play();
    }

    void Update()
    {
        if (currentNoteIndex >= noteList.Count) return;

        float elapsedTime = Time.time - startTime; // time since gameplay started
        NoteData nextNote = noteList[currentNoteIndex];

        if (elapsedTime >= nextNote.time)
        {
            SpawnNote(nextNote.lane);
            currentNoteIndex++;
        }
    }

    void SpawnNote(int lane)
    {
        Instantiate(notePrefab, laneSpawnPoints[lane].position, Quaternion.identity);
    }
}

// You'll need this data structure somewhere in your project:
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
