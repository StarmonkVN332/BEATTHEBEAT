using MidiParser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiSpawnB : MonoBehaviour
{
    public string midiFilePath = "Assets/Music/OldTownRoad.mid"; // MIDI file path
    public NoteForPoolB noteBPool;
    public float spawnHeight = 5f;
    public float noteSpeed = 1f;

    public AudioSource musicSource;
    private float startTime;
    [SerializeField] private float delayMusicStart = 3f;


    public Transform spawnOrigin;

    private List<(float time, int note, int velocity)> noteEvents = new();

    void Start()
    {
        // Load MIDI
        var midiFile = new MidiFile(midiFilePath);
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

        foreach (var note in noteEvents)
        {
            float delay = note.time - (Time.time - startTime);
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            // Tăng 2 nửa cung (tăng pitch)
            int transposedNote = note.note + 2;
            float noteOffsetX = (transposedNote - 60) * 0.2f;

            // Tạo vị trí spawn dựa trên vị trí gốc
            Vector3 spawnPos = spawnOrigin.position + new Vector3(noteOffsetX, 0, 0);

            // Spawn nốt
            GameObject noteB = noteBPool.GetNoteB();
            noteB.transform.position = spawnPos;
            noteB.transform.rotation = Quaternion.identity;

            StartCoroutine(DeactivateNote(noteB));
        }
    }
    IEnumerator DeactivateNote(GameObject noteB)
    {
        yield return new WaitForSeconds(3f);

        if (noteB != null && noteB.activeInHierarchy)
        {
            noteBPool.ReturnNoteB(noteB);
        }
    }
}
