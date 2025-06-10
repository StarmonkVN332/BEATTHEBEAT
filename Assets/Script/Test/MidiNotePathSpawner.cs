using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MidiParser;

public class MidiNotePathSpawner : MonoBehaviour
{
    [Header("MIDI File Selection")]
    public string midiFolder = "Songs";               // Folder inside Resources/
    public string midiFileName = "OldTownRoadPat1.bytes"; // Must end with .bytes

    public GameObject notePrefab;                        // Prefab của nốt nhạc
    public Transform spawnOrigin;                        // Gốc tạo nốt
    public float pathLength = 2486f;                     // Chiều dài trục Z tương ứng với độ dài bài hát
    [SerializeField] private AudioSource audioSource;
    private List<(float time, int note, int velocity)> noteEvents = new();

    void Start()
    {
        StartCoroutine(LoadAndSpawnNotesCoroutine());
    }

    IEnumerator LoadAndSpawnNotesCoroutine()
    {
        // 1. Load MIDI file
        string fullPath = Path.Combine(midiFolder, Path.GetFileNameWithoutExtension(midiFileName));
        TextAsset midiAsset = Resources.Load<TextAsset>(fullPath);

        if (midiAsset == null)
        {
            Debug.LogError($"Không tìm thấy file MIDI: {midiFileName}.bytes trong Resources");
            yield break;
        }

        MidiFile midiFile;
        try
        {
            using (var memoryStream = new MemoryStream(midiAsset.bytes))
            {
                midiFile = new MidiFile(memoryStream);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Lỗi khi đọc MIDI: {e.Message}");
            yield break;
        }

        float ticksPerQuarter = midiFile.TicksPerQuarterNote;
        float tempo = 500000f;

        // 2. Parse notes
        foreach (var track in midiFile.Tracks)
        {
            foreach (var midiEvent in track.MidiEvents)
            {
                if ((MidiEventType)midiEvent.Type == MidiEventType.MetaEvent &&
                    midiEvent.MetaEventType == MetaEventType.Tempo)
                {
                    tempo = 60000000f / midiEvent.Arg2;
                }

                if ((MidiEventType)midiEvent.Type == MidiEventType.NoteOn &&
                    midiEvent.Velocity > 0)
                {
                    float secondsPerTick = tempo / 1000000f / ticksPerQuarter;
                    float eventTime = midiEvent.Time * secondsPerTick;
                    noteEvents.Add((eventTime, midiEvent.Note, midiEvent.Velocity));
                }
            }
        }

        // 3. Spawn Notes
        SpawnNotesAlongPath();

        // 4. Wait one frame to ensure instantiation is complete
        yield return null;

        // 5. Start audio and unlock game
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // ✅ Allow game to start
        GameState.GameStarted = true;
    }

    void SpawnNotesAlongPath()
{
    if (noteEvents.Count == 0 || notePrefab == null)
        return;

    float songDuration = noteEvents[noteEvents.Count - 1].time;

    foreach (var note in noteEvents)
    {
        // Chuyển note theo pitch
        int transposedNote = note.note + 2;
        float offsetX = (transposedNote - 60) * 0.2f;

        // Tính vị trí Z theo thời gian
        float zPos = (note.time / songDuration) * pathLength;

        // Xác định vị trí spawn
        Vector3 spawnPos = spawnOrigin.position + new Vector3(offsetX, 0, zPos);

        // Tạo đối tượng mới từ Prefab
        Instantiate(notePrefab, spawnPos, Quaternion.identity);
    }

    // ✅ Play audio after all notes have been instantiated
    if (audioSource != null)
    {
        audioSource.Play();
    }
}
}
