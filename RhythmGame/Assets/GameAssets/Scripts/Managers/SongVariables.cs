using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void OnInfoReceived()
    {
        GetSongInfo();
    }

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