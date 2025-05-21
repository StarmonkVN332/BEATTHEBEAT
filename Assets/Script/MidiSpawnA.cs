using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MidiParser;

public class MidiSpawnA : MonoBehaviour
{
    public string midiResourceName = "OldTownRoadPat1"; // Name of the .bytes file (without extension) in Resources
    public NoteForPoolA notePoolA;
    public float spawnHeight = 5f;
    public float noteSpeed = 1f;

    public AudioSource musicSource;
    private float startTime;
    [SerializeField] private float delayMusicStart = 3f;

    public Transform spawnOrigin;

    private List<(float time, int note, int velocity)> noteEvents = new();

    void Start()
    {
        // Load .bytes file as TextAsset from Resources
        TextAsset midiAsset = Resources.Load<TextAsset>(midiResourceName);
        if (midiAsset == null)
        {
            Debug.LogError($"Failed to load MIDI file: {midiResourceName}.bytes from Resources");
            return;
        }

        // Parse MIDI file from TextAsset bytes
        MidiFile midiFile;
        try
        {
            using (var memoryStream = new MemoryStream(midiAsset.bytes))
            {
                midiFile = new MidiFile(memoryStream); // Assumes MidiFile supports Stream input
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing MIDI file: {e.Message}");
            return;
        }

        StartMusicWithDelay();
        float ticksPerQuarter = midiFile.TicksPerQuarterNote;
        float tempo = 500000f; // default 120bpm

        foreach (var track in midiFile.Tracks)
        {
            foreach (var midiEvent in track.MidiEvents)
            {
                if ((MidiEventType)midiEvent.Type == MidiEventType.MetaEvent && midiEvent.MetaEventType == MetaEventType.Tempo)
                {
                    // Cập nhật tempo
                    tempo = 60000000f / midiEvent.Arg2;
                }

                if ((MidiEventType)midiEvent.Type == MidiEventType.NoteOn && midiEvent.Velocity > 0)
                {
                    float secondsPerTick = tempo / 1000000f / ticksPerQuarter;
                    float eventTime = midiEvent.Time * secondsPerTick;
                    noteEvents.Add((eventTime, midiEvent.Note, midiEvent.Velocity));
                }
            }
        }

        // Bắt đầu coroutine spawn
        StartCoroutine(SpawnNotes());
    }

    private void StartMusicWithDelay()
    {
        // Lấy thời gian hiện tại cộng với độ trễ (delay)
        double startTime = AudioSettings.dspTime + delayMusicStart;

        // Đặt thời điểm phát nhạc chính xác (mượt mà hơn)
        musicSource.PlayScheduled(startTime);
    }

    IEnumerator SpawnNotes()
    {
        float startTime = Time.time;

        foreach (var noteA in noteEvents)
        {
            float delay = noteA.time - (Time.time - startTime);
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            // Tăng 2 nửa cung (tăng pitch)
            int transposedNote = noteA.note + 2;
            float noteOffsetX = (transposedNote - 60) * 0.2f;

            // Tạo vị trí spawn dựa trên vị trí gốc
            Vector3 spawnPos = spawnOrigin.position + new Vector3(noteOffsetX, 0, 0);

            // Spawn nốt
            GameObject noteObjA = notePoolA.GetNoteA();
            noteObjA.transform.position = spawnPos;
            noteObjA.transform.rotation = Quaternion.identity;

            StartCoroutine(DeactivateNoteA(noteObjA));
        }
    }

    IEnumerator DeactivateNoteA(GameObject noteObj)
    {
        yield return new WaitForSeconds(3f);

        if (noteObj != null && noteObj.activeInHierarchy)
        {
            notePoolA.ReturnNoteA(noteObj);
        }
    }
}
