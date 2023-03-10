using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Debug = UnityEngine.Debug;

// using Note = Melanchall.DryWetMidi.MusicTheory.Note;

public class NoteManager : MonoBehaviour
{
    public float noteSpeedDistMultiplier;
    public static MidiFile songChart;
    public List<string> chartNames = new();
    public static Note[] notesArray;
    int index;

    void OnEnable()
    {
        AudioManager.onInfoReceived += OnInfoReceived;
    }

    void OnDisable()
    {
        AudioManager.onInfoReceived -= OnInfoReceived;
    }

    void Start()
    {
        StartCoroutine(SmallWait());
    }

    IEnumerator SmallWait() //this coroutine is used to prevent a nullreference that shouldnt be happening,
        //but does happen for some reason.
    {
        yield return new WaitForSeconds(0.1f);
        index = AudioManager.instance.songIndex;
    }

    /// <summary>
    /// Locates and assigns the MIDI(.mid) file
    /// so that the notes can be read and used.
    /// </summary>
    void ReadFromFile()
    {
        string fileDir = $"{Application.dataPath}/StreamingAssets/{chartNames[index]}.mid";
        songChart = MidiFile.Read(fileDir);
        GetMidiData();
    }

    /// <summary>
    /// Reads the data from the assigned MIDI file,
    /// placing all the notes in an array.
    /// </summary>
    void GetMidiData()
    {
        if (songChart == null)
        {
            Debug.Log("Chart not found");
            return;
        }

        songChart.ReplaceTempoMap(TempoMap.Create(Tempo.FromBeatsPerMinute(AudioManager.bpm)));
        var notes = songChart.GetNotes();
        notesArray = new Note[notes.Count];
        notes.CopyTo(notesArray, 0);
        NotePooler.CopyNoteList(notesArray);
    }

    void OnInfoReceived()
    {
        ReadFromFile();
    }
}