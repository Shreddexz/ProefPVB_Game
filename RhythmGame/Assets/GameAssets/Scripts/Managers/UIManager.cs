using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    ScoreManager scoreManager;
    [Header("Score variables")] public TextMeshProUGUI scoreText, scoreTextP2;
    public TextMeshProUGUI multiText, multiTextP2;

    [Header("UI Images")] public Image timefill;

    public Image multi2xFill,
                 multi4xFill,
                 multi8xFill,
                 multi2xFillP2,
                 multi4xFillP2,
                 multi8xFillP2;


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
        multi2xFill.fillAmount =
            (float) scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[1];
        multi4xFill.fillAmount =
            (float) scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[2];
        multi8xFill.fillAmount =
            (float) scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[^1];

        if (!PlayerManager.multiplayer)
            return;

        scoreTextP2.text = scoreManager.p2Score.score.ToString();
        multiTextP2.text = $"{scoreManager.p2Score.multiplier.ToString()}x";
        multi2xFillP2.fillAmount =
            (float) scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[1];
        multi4xFillP2.fillAmount =
            (float) scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[2];
        multi8xFillP2.fillAmount =
            (float) scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[^1];
    }

    void SongElements()
    {
        timefill.fillAmount =
            ((float) SongVariables.playbackTime / (AudioManager.songLength / 10)) * 100;
    }
}