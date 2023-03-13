using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using Debug = UnityEngine.Debug;

public class NotePooler : MonoBehaviour
{
    public int notePoolLimit = 15;
    public GameObject noteEnemy;
    double songTime;
    float spawnOffset;
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
    NoteManager manager;

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
        AudioManager.masterSystem.getDSPBufferSize(out uint bufferLength, out int numBuffers);
        audioLatencyMS = (double) numBuffers * bufferLength / AudioManager.instance.masterSampleRate;
    }

    void Awake()
    {
        manager = transform.GetComponent<NoteManager>();
        lanes = new Lane[lanesCopy.Length];
        bpmSet = false;
        timeStamps = new();
        lanesCopy.CopyTo(lanes, 0);
    }

    void GetSongBPM()
    {
        if (AudioManager.bpm != 0f)
        {
            spawnOffset = ((60 * 16 / SongVariables.bpm) * manager.noteSpeedDistMultiplier + 3f);
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
            }

            lanes[laneIndex].timeStamps.Add(noteTime);
        }
    }


    public void SpawnNote(Lane lane)
    {
        if (pooledNotes.Count > 0)
        {
            NoteEnemy note = pooledNotes[0];
            if (note.transform.position != lane.gameObject.transform.position)
                note.transform.position = lane.gameObject.transform.position;
            note.transform.parent = lane.transform;
            note.canMove = true;
            note.gameObject.SetActive(true);
            note.NotePlaced();
            pooledNotes.Remove(note);
            lane.activeNotes.Add(note);
            return;
        }

        GameObject spawnedNote = Instantiate(noteEnemy, lane.gameObject.transform);

        lane.activeNotes.Add(spawnedNote.GetComponent<NoteEnemy>());
    }

    public void PoolObject(NoteEnemy noteObj)
    {
        Lane parentLane = noteObj.transform.parent.GetComponent<Lane>();
        parentLane.activeNotes.Remove(noteObj);

        if (pooledNotes.Count >= notePoolLimit)
        {
            Destroy(noteObj.gameObject);
            return;
        }

        noteObj.gameObject.SetActive(false);
        noteObj.canMove = false;
        pooledNotes.Add(noteObj);
        noteObj.transform.parent = null;
    }
}