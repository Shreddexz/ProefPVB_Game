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

    public Image splitBar;
    public RectTransform readyPanel;
    public TextMeshProUGUI p1ready, p2ready;

    private void OnEnable()
    {
        PlayerManager.AllReady += OnPlayersReady;
    }

    private void OnDisable()
    {
        PlayerManager.AllReady -= OnPlayersReady;
    }

    void OnPlayersReady()
    {
        readyPanel.gameObject.SetActive(false);
    }

    void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        if (PlayerManager.multiplayer) splitBar.gameObject.SetActive(true);

        ReadyPanel();
    }

    void Update()
    {
        PlayerScore();
        SongElements();
        ReadyPanel();
    }
    void ReadyPanel()
    {
        if (!PlayerManager.playersReady)
        {
            if (!readyPanel.gameObject.activeInHierarchy)
                readyPanel.gameObject.SetActive(true);

            p1ready.text = PlayerManager.player1Ready ? "Player 1 is ready" : "Waiting for player 1 to ready up";
            if (PlayerManager.multiplayer)
            {
                if (!p2ready.gameObject.activeInHierarchy)
                    p2ready.gameObject.SetActive(true);

                p2ready.text = PlayerManager.player2Ready ? "Player 2 is ready" : "Waiting for player 2 to ready up";
            }
        }
    }

    void PlayerScore()
    {
        scoreText.text = scoreManager.p1Score.score.ToString();
        multiText.text = $"{scoreManager.p1Score.multiplier.ToString()}x";
        multi2xFill.fillAmount =
            (float)scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[1];
        multi4xFill.fillAmount =
            (float)scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[2];
        multi8xFill.fillAmount =
            (float)scoreManager.p1Score.multiplierValue / scoreManager.multiplierThresholds[^1];

        if (!PlayerManager.multiplayer)
            return;

        scoreTextP2.text = scoreManager.p2Score.score.ToString();
        multiTextP2.text = $"{scoreManager.p2Score.multiplier.ToString()}x";
        multi2xFillP2.fillAmount =
            (float)scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[1];
        multi4xFillP2.fillAmount =
            (float)scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[2];
        multi8xFillP2.fillAmount =
            (float)scoreManager.p2Score.multiplierValue / scoreManager.multiplierThresholds[^1];
    }

    void SongElements()
    {
        timefill.fillAmount =
            ((float)SongVariables.playbackTime / (AudioManager.songLength / 10)) * 100;
    }
}