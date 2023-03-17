using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class SongButtons : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI songNameText;
    public TextMeshProUGUI songDurationText;
    public TextMeshProUGUI songDifText;

    public string songName;
    public float songDurationSeconds;
    public songDifficulty difficulty;

    public void OnSelect(BaseEventData eventData)
    {
        ShowSongInfo();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        songNameText.text = string.Empty;
        songDurationText.text = string.Empty;
        songDifText.text = string.Empty;
    }

    public void ShowSongInfo()
    {
        songNameText.text = songName;
        TimeSpan songDurTS = TimeSpan.FromSeconds(songDurationSeconds);
        songDurationText.text = songDurTS.ToString("mm':'ss");
        songDifText.text = difficulty.ToString();
    }

    public enum songDifficulty
    {
        easy,
        medium,
        hard
    }
}
