using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MidiParser;
using Unity.VisualScripting;

public class MidiNotePathSpawner : MonoBehaviour
{
    [Header("MIDI File Selection")]
    public string midiFolder = "Songs";                     // Thư mục trong Resources/
    public string midiFileName = "OldTownRoadPat1.bytes";   // File MIDI (đuôi .bytes)

    public Transform spawnOrigin;                           // Gốc tạo nốt
    public float pathLength = 2486f;                        // Chiều dài trục Z tương ứng với độ dài bài hát
    [SerializeField] private AudioSource audioSource;

    private List<(float time, int note, int velocity)> noteEvents = new();
    [SerializeField] private NotePool notePool;

    void Start()
    {
        notePool = NotePool.Instance;
        if (notePool == null)
        {
            Debug.LogError("Không tìm thấy NotePool trong scene!");
            return;
        }

        StartCoroutine(LoadAndSpawnNotesCoroutine());
    }

    IEnumerator LoadAndSpawnNotesCoroutine()
    {
        // 1. Load MIDI
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

        yield return null;

        // 4. Play audio
        
    }

    void SpawnNotesAlongPath()
    {
        if (noteEvents.Count == 0 || notePool == null)
            return;

        float songDuration = noteEvents[noteEvents.Count - 1].time;

        foreach (var note in noteEvents)
        {
            SpawnSingleNote(note, songDuration);
        }
        StartCoroutine(GameStart());
    }

    void SpawnSingleNote((float time, int note, int velocity) noteData, float songDuration)
    {
        int transposedNote = noteData.note + 2; // Nếu cần transpose
        float offsetX = (transposedNote - 60) * 0.2f;
        float zPos = (noteData.time / songDuration) * pathLength;
        Vector3 spawnPos = spawnOrigin.position + new Vector3(offsetX, 0, zPos);

        GameObject noteObj = notePool.GetNote();
        noteObj.transform.position = spawnPos;

        // (Nếu cần reset animation, script riêng, bạn có thể gọi tại đây)
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(5f);
        GameState.GameStarted = true;
        Debug.Log("Game Bắt đầu");
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Chơi nhạc");
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
