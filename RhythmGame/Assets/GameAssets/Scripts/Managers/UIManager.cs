using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    ScoreManager scoreManager;
    [Header("Score variables")] public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;

    [Header("UI Images")] public Image timefill;
    public Image multiplierFill;


    void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }

    void Update()
    {
        PlayerScore();
        SongElements();
    }

    void PlayerScore()
    {
        scoreText.text = scoreManager.p1Score.score.ToString();
        multiText.text = $"{scoreManager.p1Score.multiplier.ToString()}x";
        multiplierFill.fillAmount =
            (float) scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[^1];
    }

    void SongElements()
    {
        timefill.fillAmount =
            ((float) SongVariables.playbackTime / AudioManager.songLength * 48000 / 1000 / 4.78f) * 100;
    }
}