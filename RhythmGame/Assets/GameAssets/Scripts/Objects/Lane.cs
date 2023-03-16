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
    public KeyCode laneKeyP1, laneKeyP2;
    public NoteName laneNote;
    public List<double> timeStamps = new();
    public List<NoteEnemy> activeNotes;
    public List<NoteEnemy> activeNotes2;
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

        ListNullCheck();
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

        double hitTime = playerID == 0 ? activeNotes[0].arriveTime - inputTime : activeNotes2[0].arriveTime - inputTime;

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
            switch (playerID)
            {
                case 0:
                    activeNotes[0].Destroy();
                    activeNotes.Remove(activeNotes[0]);
                    break;

                case 1:
                    activeNotes2[0].Destroy();
                    activeNotes.Remove(activeNotes2[0]);
                    break;
            }

            noteHit = false;
        }
    }

    void ListNullCheck()
    {
        for (int i = 0; i < activeNotes.Count; i++)
        {
            if (activeNotes[i] == null)
                activeNotes.RemoveAt(i);
        }

        for (int i = 0; i < activeNotes2.Count; i++)
        {
            if (activeNotes2[i] == null)
                activeNotes2.RemoveAt(i);
        }
    }
}