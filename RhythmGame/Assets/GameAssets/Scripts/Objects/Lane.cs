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

    public GameObject noteObject;
    public List<NoteEnemy> notes = new();
    public List<double> timeStamps = new();
    [NonSerialized]
    public int noteIndex;
    int inputIndex;

    NotePooler pooler;

    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
    }

    ///SPAWN CODE IN HERE REFFERING TO POOLER
    
    // void Update()
    // {
    //
    //     if (inputIndex < timeStamps.Count)
    //     {
    //         double timeStamp = timeStamps[inputIndex];
    //         double errorMargin = AudioManager.instance.marginOfErrorSeconds;
    //         double audioTime = AudioManager.instance.playbackTime -
    //                            (AudioManager.instance.inputDelayMilliseconds / 1000f);
    //
    //         if (Input.GetKeyDown(KeyCode.Alpha1))
    //         {
    //             if (Math.Abs(audioTime - timeStamp) < errorMargin)
    //                 Hit();
    //         }
    //
    //         if (timeStamp + errorMargin <= audioTime)
    //         {
    //             Miss();
    //         }
    //     }
    //     
    // }
    //
    // void Hit()
    // {
    //     Debug.Log("Note hit");
    // }
    //
    // void Miss()
    // {
    //     Debug.Log("Note Missed");
    //     inputIndex++;
    // }
}