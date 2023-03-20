using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Displays info about the song that's being hovered on in the menu
/// </summary>
public class SongButtons : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI songNameText;
    public TextMeshProUGUI songDurationText;
    public TextMeshProUGUI songDifText;

    public string songName;
    public float songDurationSeconds;
    public songDifficulty difficulty;

    /// <summary>
    /// When hovering on this item, a method gets called to display the data in the UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        ShowSongInfo();
    }

    /// <summary>
    /// when hovering stops, remove data from UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        songNameText.text = string.Empty;
        songDurationText.text = string.Empty;
        songDifText.text = string.Empty;
    }

    /// <summary>
    /// Display data from this song in the UI
    /// </summary>
    public void ShowSongInfo()
    {
        songNameText.text = songName;
        TimeSpan songDurTS = TimeSpan.FromSeconds(songDurationSeconds);
        songDurationText.text = songDurTS.ToString("mm':'ss");
        songDifText.text = difficulty.ToString();
    }


    /// <summary>
    /// Difficulty selection in a dropdown
    /// </summary>
    public enum songDifficulty
    {
        easy,
        medium,
        hard
    }
}
