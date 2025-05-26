using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MidiParser;

public class MidiNotePathSpawner : MonoBehaviour
{
    public string midiResourceName = "OldTownRoadPat1"; // Tên file MIDI (.bytes) trong Resources
    public GameObject notePrefab;                        // Prefab của nốt nhạc
    public Transform spawnOrigin;                        // Gốc tạo nốt
    public float pathLength = 2486f;                     // Chiều dài trục Z tương ứng với độ dài bài hát

    private List<(float time, int note, int velocity)> noteEvents = new();

    void Start()
    {
        // Load file MIDI (.bytes)
        TextAsset midiAsset = Resources.Load<TextAsset>(midiResourceName);
        if (midiAsset == null)
        {
            Debug.LogError($"Không tìm thấy file MIDI: {midiResourceName}.bytes trong Resources");
            return;
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
            return;
        }

        float ticksPerQuarter = midiFile.TicksPerQuarterNote;
        float tempo = 500000f; // tempo mặc định 120 bpm

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

        SpawnNotesAlongPath();
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
    }
}
