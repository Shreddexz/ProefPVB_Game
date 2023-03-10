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
    public KeyCode laneKey;
    public NoteName laneNote;
    public List<double> timeStamps = new();
    public List<NoteEnemy> activeNotes;
    int noteIndex;
    NotePooler pooler;
    bool noteHit;

    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
        activeNotes = new();
    }

    void Update()
    {
        if (timeStamps.Count > 0)
            CheckNoteConditions();
    }

    void CheckNoteConditions()
    {
        if (AudioManager.playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            return;

        if (noteIndex >= timeStamps.Count)
            return;

        if (AudioManager.instance.playbackTime >= timeStamps[noteIndex] - SongVariables.twoBarsDuration)
        {
            pooler.SpawnNote(this);
            noteIndex++;
        }
    }

    public void NotePressed(double inputTime)
    {
        if (!SongVariables.infoSet)
            return;

        double hitTime = activeNotes[0].arriveTime - inputTime;

        if (Math.Abs(hitTime) <=
            SongVariables.quarterNoteDuration)
        {
            Debug.Log("Perfect");
            Debug.Log($"{Math.Abs(hitTime)} | {SongVariables.quarterNoteDuration}");
            noteHit = true;
        }
        else if (Math.Abs(hitTime) <= SongVariables.halfNoteDuration)
        {
            Debug.Log("Good");
            Debug.Log($"{Math.Abs(hitTime)} | {SongVariables.halfNoteDuration}");
            noteHit = true;
        }
        else if (Math.Abs(hitTime) <= SongVariables.noteDuration)
        {
            Debug.Log("OK");
            Debug.Log($"{Math.Abs(hitTime)} | {SongVariables.noteDuration}");
            noteHit = true;
        }
        else
        {
            Debug.Log("Miss");
        }

        if (noteHit)
        {
            pooler.PoolObject(activeNotes[0]);
            noteHit = false;
        }
    }
}