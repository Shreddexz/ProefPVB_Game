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
    public NoteName laneNote;
    public List<double> timeStamps = new();
    int noteIndex;
    NotePooler pooler;

    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
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

        if (AudioManager.instance.playbackTime >= timeStamps[noteIndex] - NotePooler.twoBarsDuration)
        {
            pooler.SpawnNote(this);
            noteIndex++;
        }
    }
}