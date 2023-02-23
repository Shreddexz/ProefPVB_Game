using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

public class NotePooler : MonoBehaviour
{
    public int notePoolLimit = 15;
    GameObject noteEnemy;
    double songTime;
    double twoBarsDuration;
    float spawnOffset;
    float bpm;
    public static Note[] notesArray;
    public List<NoteEnemy> pooledNotes = new();
    bool bpmSet;

    public Transform spawnPos;

    void Awake()
    {
        bpmSet = false;
    }

    void GetSongBPM()
    {
        if (AudioManager.bpm != 0f)
        {
            Debug.Log(AudioManager.bpm);
            bpm = AudioManager.bpm;
            spawnOffset = 60 * 8 / bpm;
            twoBarsDuration = 60 * 8 / bpm;
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
        // foreach (var note in notesArray)
        // {
        //     MetricTimeSpan mts =
        //         TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, NoteManager.songChart.GetTempoMap());
        //     double noteTime = ((double) mts.Minutes * 60) + ((double) mts.Seconds) +
        //                       ((double) mts.Milliseconds / 1000);
        // }
    }

    void CheckNoteConditions()
    {
        // if (AudioManager.playbackState == PLAYBACK_STATE.PLAYING)
        //     foreach (var note in notesArray)
        //     {
        //         MetricTimeSpan mts =
        //             TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, NoteManager.songChart.GetTempoMap());
        //         double noteTime = ((double) mts.Minutes * 60) + ((double) mts.Seconds) +
        //                           ((double) mts.Milliseconds / 1000);
        //     }

        for (int i = 0; i < notesArray.Length - 1; i++)
        {
            if (songTime >= notesArray[i].Time - twoBarsDuration)
            {
                Debug.Log("test");
                SpawnNote();
            }
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

        GameObject noteObj = Instantiate(noteEnemy, spawnPos);
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