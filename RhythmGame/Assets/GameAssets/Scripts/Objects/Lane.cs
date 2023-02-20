using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Note = Melanchall.DryWetMidi.Interaction.Note;

public class Lane : MonoBehaviour
{
    public NoteName LaneNote;

    public GameObject noteObject;
    List<NoteEnemy> notes = new();
    public List<double> timeStamps = new();
    int spawnIndex;
    int inputIndex;

    void Start()
    {
    }

    public void SetTimestamp(Note[] noteArray)
    {
        foreach (var note in noteArray)
        {
            if (note.NoteName == LaneNote)
            {
                MetricTimeSpan mts =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, AudioManager.songChart.GetTempoMap());
                timeStamps.Add((double) mts.Minutes * 60f + mts.Seconds + (double) mts.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (AudioManager.instance.pbt >=
                timeStamps[spawnIndex] - AudioManager.instance.noteTime)
            {
                GameObject note = Instantiate(noteObject, transform);
                notes.Add(note.GetComponent<NoteEnemy>());
                note.GetComponent<NoteEnemy>().arriveTime = (float) timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double errorMargin = AudioManager.instance.marginOfErrorSeconds;
            double audioTime = AudioManager.instance.pbt -
                               (AudioManager.instance.inputDelayMilliseconds / 1000f);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Math.Abs(audioTime - timeStamp) < errorMargin)
                    Hit();
            }

            if (timeStamp + errorMargin <= audioTime)
            {
                Miss();
            }
        }
        
    }

    void Hit()
    {
        Debug.Log("Note hit");
    }

    void Miss()
    {
        Debug.Log("Note Missed");
        inputIndex++;
    }
}