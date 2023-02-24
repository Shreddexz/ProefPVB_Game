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
    double twoBarsDuration;
    float spawnOffset;
    float bpm;
    public static Note[] notesArray;
    public List<NoteEnemy> pooledNotes = new();
    static List<double> timeStamps;
    bool bpmSet;
    int noteIndex;

    public Transform spawnPos;

    void Awake()
    {
        bpmSet = false;
        timeStamps = new();
    }

    void GetSongBPM()
    {
        if (AudioManager.bpm != 0f)
        {
            Debug.Log(AudioManager.bpm);
            bpm = AudioManager.bpm;
            twoBarsDuration = 60 * 8 / bpm;
            spawnOffset = 60 * 16 / bpm;
            spawnPos.position = new Vector3(0f, 0f, spawnOffset);
            bpmSet = true;
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
            MetricTimeSpan mts = note.TimeAs<MetricTimeSpan>(NoteManager.songChart.GetTempoMap());
            double noteTime = Convert.ToDouble(mts.Minutes * 60) + Convert.ToDouble(mts.Seconds) +
                              Convert.ToDouble(mts.Milliseconds / 1000);
            timeStamps.Add(noteTime);
            Debug.Log(noteTime);
        }
    }


    void CheckNoteConditions()
    {
        if (AudioManager.playbackState == PLAYBACK_STATE.PLAYING)
            if (noteIndex < timeStamps.Count)
                if (songTime >= timeStamps[noteIndex] - twoBarsDuration)
                {
                    SpawnNote();
                    noteIndex++;
                }
    }

    public void SpawnNote()
    {
        if (pooledNotes.Count > 0)
        {
            NoteEnemy note = pooledNotes[0];
            if (note.transform.position != spawnPos.transform.position)
                note.transform.position = spawnPos.transform.position;
            note.canMove = true;
            note.gameObject.SetActive(true);
            pooledNotes.Remove(note);
            return;
        }

        Instantiate(noteEnemy, spawnPos);
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
        noteObj.transform.position = spawnPos.transform.position;
    }
}