using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

// using Note = Melanchall.DryWetMidi.MusicTheory.Note;

public class NoteManager : MonoBehaviour
{
    public static MidiFile songChart;
    public List<string> chartNames = new();
    public static Note[] notesArray;
    int index = 0;

    void Start()
    {
        ReadFromFile();
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
        var notes = songChart.GetNotes();
        notesArray = new Note[notes.Count];
        notes.CopyTo(notesArray, 0);
        NotePooler.CopyNoteList(notesArray);
    }

    void Update()
    {
    }
}