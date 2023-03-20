using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class globalises the variables set, to make sure other classes that need to access it all have the same values.
/// </summary>
public class SongVariables : MonoBehaviour
{
    public static double playbackTime,
                         songBarDuration,
                         twoBarsDuration,
                         quarterNoteDuration,
                         halfNoteDuration,
                         noteDuration;

    public static float bpm;
    public static bool infoSet;

    void OnEnable()
    {
        AudioManager.onInfoReceived += OnInfoReceived;
    }

    void OnDisable()
    {
        AudioManager.onInfoReceived -= OnInfoReceived;
    }

    /// <summary>
    /// when the FMOD timeline data is retrieved, a method is called to set all the necessary variables.
    /// </summary>
    void OnInfoReceived()
    {
        GetSongInfo();
    }

    /// <summary>
    /// Sets all the variables necessary based on the song's BPM.
    /// This is done to make sure everything will be in sync with the current song.
    /// </summary>
    void GetSongInfo()
    {
        bpm = AudioManager.bpm;
        songBarDuration = (60 * 4) / bpm;
        twoBarsDuration = songBarDuration * 2;
        noteDuration = bpm < 150 ? 60 / bpm / 2 : 60 / bpm;
        halfNoteDuration = noteDuration / 2;
        quarterNoteDuration = noteDuration / 4;
        infoSet = true;
    }

    void Update()
    {
        if (infoSet)
            playbackTime = AudioManager.instance.playbackTime;
    }
}