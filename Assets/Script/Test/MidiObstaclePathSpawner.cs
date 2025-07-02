using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MidiParser;
using UnityEditor.Experimental.GraphView;

public class MidiObstaclePathSpawner : MonoBehaviour
{
    [Header("MIDI File Selection")]
    public string midiFolder = "Songs";
    public string midiFileName = "OldTownRoadPat1.bytes";

    public Transform spawnOrigin;
    public float pathLength = 2486f;
    [SerializeField] private AudioSource audioSource;

    private List<(float time, int note, int velocity)> noteEvents = new();

    [SerializeField] private GameObject obstacle;
    private Transform obstacleRotation;
    void Start()
    {
        StartCoroutine(LoadAndSpawnObstaclesCoroutine());
        
    }

    IEnumerator LoadAndSpawnObstaclesCoroutine()
    {
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

        SpawnObstaclesAlongPath();

        yield return null;

        
    }

    void SpawnObstaclesAlongPath()
    {
        if (noteEvents.Count == 0)
            return;

        float songDuration = noteEvents[noteEvents.Count - 1].time;

        foreach (var note in noteEvents)
        {
            SpawnSingleObstacle(note, songDuration);
        }
        //if (audioSource != null)
        //{
        //    audioSource.Play();
        //}

        //GameState.GameStarted = true;
    }

    void SpawnSingleObstacle((float time, int note, int velocity) noteData, float songDuration)
    {
        int transposedNote = noteData.note + 2; // Nếu cần transpose
        float offsetX = (transposedNote - 60) * 0.2f;
        float zPos = (noteData.time / songDuration) * pathLength;
        Vector3 spawnPos = spawnOrigin.position + new Vector3(offsetX, 0, zPos);

        GameObject obstacleObj = Instantiate(obstacle, spawnPos, obstacle.transform.rotation);

    }
}
