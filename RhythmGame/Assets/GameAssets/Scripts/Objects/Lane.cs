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
    ScoreManager scoreManager;
    bool noteHit;

    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
        scoreManager = transform.root.GetComponent<ScoreManager>();
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

    public void NotePressed(int playerID, double inputTime)
    {
        if (!SongVariables.infoSet)
            return;

        if (activeNotes.Count <= 0)
        {
            scoreManager.ReduceMultiplier(playerID);
            return;
        }

        double hitTime = activeNotes[0].arriveTime - inputTime;

        if (Math.Abs(hitTime) <=
            SongVariables.quarterNoteDuration)
        {
            scoreManager.AddScore(playerID, "Perfect");
            noteHit = true;
        }
        else if (Math.Abs(hitTime) <= SongVariables.halfNoteDuration)
        {
            scoreManager.AddScore(playerID, "Good");
            noteHit = true;
        }
        else if (Math.Abs(hitTime) <= SongVariables.noteDuration)
        {
            scoreManager.AddScore(playerID, "OK");
            noteHit = true;
        }
        else
        {
            scoreManager.ReduceMultiplier(playerID);
        }


        if (noteHit)
        {
            pooler.PoolObject(activeNotes[0]);
            noteHit = false;
        }
    }
}