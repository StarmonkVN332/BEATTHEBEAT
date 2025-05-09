using System.Collections.Generic;
using UnityEngine;

public class NoteS : MonoBehaviour
{
    public GameObject notePrefab;
    public AudioSource musicSource;
    public Transform[] laneSpawnPoints; // 0 = left, 1 = middle, 2 = right
    public TextAsset beatmapJson;

    private List<NoteData> noteList;
    private int currentNoteIndex = 0;

    void Start()
    {
        Beatmap beatmap = JsonUtility.FromJson<Beatmap>(beatmapJson.text);
        noteList = beatmap.notes;
    }

    void Update()
    {
        if (currentNoteIndex >= noteList.Count) return;

        float currentTime = musicSource.time + 1f;
        NoteData nextNote = noteList[currentNoteIndex];

        if (currentTime >= nextNote.time)
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
