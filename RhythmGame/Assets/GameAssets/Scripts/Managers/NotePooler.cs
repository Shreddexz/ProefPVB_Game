using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using Debug = UnityEngine.Debug;

public class NotePooler : MonoBehaviour
{
    public int notePoolLimit = 15;
    public GameObject noteEnemy;
    double songTime;
    public static double twoBarsDuration;
    float spawnOffset;
    float bpm;
    public static Note[] notesArray;
    public List<NoteEnemy> pooledNotes = new();
    static List<double> timeStamps;
    static double audioLatencyMS;
    bool bpmSet;
    int noteIndex;
    static string noteConcat;
    public static Lane[] lanes;
    public Lane[] lanesCopy;
    static int laneIndex;

    public Transform spawnPos;

    void OnEnable()
    {
        AudioManager.onMusicStart += OnInfoReceived;
    }

    void OnDisable()
    {
        AudioManager.onMusicStart -= OnInfoReceived;
    }

    void OnInfoReceived()
    {
        AudioManager.masterSystem.getDSPBufferSize(out uint bufferlength, out int numbuffers);
        audioLatencyMS = (double) numbuffers * bufferlength / AudioManager.instance.masterSampleRate;
        Debug.Log($"Audio latency = {audioLatencyMS * 1000}ms");
    }

    void Awake()
    {
        lanes = new Lane[lanesCopy.Length];
        bpmSet = false;
        timeStamps = new();
        lanesCopy.CopyTo(lanes, 0);
    }

    void GetSongBPM()
    {
        if (AudioManager.bpm != 0f)
        {
            Debug.Log(AudioManager.bpm);
            bpm = AudioManager.bpm;
            twoBarsDuration = 60 * 8 / bpm;
            spawnOffset = 60 * 16 / bpm;
            // spawnPos.position = new Vector3(0f, 0f, spawnOffset);
            bpmSet = true;
            foreach (var lane in lanes)
            {
                lane.transform.position = new Vector3(lane.transform.position.x, 0, spawnOffset);
            }
        }
    }

    void Update()
    {
        if (!bpmSet)
            GetSongBPM();

        songTime = AudioManager.instance.playbackTime;
        CheckNoteConditions();
    }

    public static void CopyNoteList(Note[] arrayToCopy)
    {
        notesArray = arrayToCopy;
        AddNotesTimes();
    }

    static void AddNotesTimes()
    {
        foreach (var note in notesArray)
        {
            double noteTime = note.TimeAs<MetricTimeSpan>(NoteManager.songChart.GetTempoMap()).TotalSeconds +
                              audioLatencyMS;

            noteConcat = String.Concat(note.NoteName, note.Octave);
            Debug.Log($"{noteConcat}, {noteTime}");
            switch (noteConcat)
            {
                case "C4":
                    laneIndex = 0;
                    break;

                case "D4":
                    laneIndex = 1;
                    break;

                case "E4":
                    laneIndex = 2;
                    break;

                default:
                    laneIndex = 0;
                    break;
            }

            lanes[laneIndex].timeStamps.Add(noteTime);
            // timeStamps.Add(noteTime);
        }
    }


    void CheckNoteConditions()
    {
        // if (AudioManager.playbackState == PLAYBACK_STATE.PLAYING)
        //     for (int i = 0; i < lanes.Length; i++)
        //     {
        //         if (lanes[i].noteIndex < lanes[i].timeStamps.Count)
        //             if (songTime >= lanes[i].timeStamps[noteIndex] - twoBarsDuration)
        //             {
        //                 SpawnNote(lanes[i]);
        //                 lanes[i].noteIndex++;
        //             }
        //     }
    }

    public void SpawnNote(Lane lane)
    {
        if (pooledNotes.Count > 0)
        {
            NoteEnemy note = pooledNotes[0];
            if (note.transform.position != lane.gameObject.transform.position)
                note.transform.position = lane.gameObject.transform.position;
            note.canMove = true;
            note.gameObject.SetActive(true);
            pooledNotes.Remove(note);
            return;
        }

        Instantiate(noteEnemy, lane.gameObject.transform);
    }

    public void PoolObject(NoteEnemy noteObj)
    {
        if (pooledNotes.Count >= notePoolLimit)
        {
            Destroy(noteObj.gameObject);
            return;
        }

        noteObj.canMove = false;
        pooledNotes.Add(noteObj);
        noteObj.gameObject.SetActive(false);
        // noteObj.transform.position = spawnPos.transform.position;
    }
}