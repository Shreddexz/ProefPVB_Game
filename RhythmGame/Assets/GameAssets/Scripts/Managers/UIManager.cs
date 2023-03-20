using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    [Header("ScoreScreen")]
    public RectTransform scoreScreen;
    public RectTransform p2ScoreTab;
    public TextMeshProUGUI p1score, p2score;
    public TextMeshProUGUI p1streak, p2streak;
    public TextMeshProUGUI p1hstreak, p2hstreak;
    public TextMeshProUGUI p1hit, p2hit;
    public TextMeshProUGUI p1missed, p2missed;
    public TextMeshProUGUI winnerText;
    private void OnEnable()
    {
        PlayerManager.AllReady += OnPlayersReady;
        AudioManager.OnMusicStopped += OnGameEnd;
    }

    private void OnDisable()
    {
        PlayerManager.AllReady -= OnPlayersReady;
        AudioManager.OnMusicStopped -= OnGameEnd;

    }

    /// <summary>
    /// This method is called when the players are readied, which then hides the ready-up panel
    /// </summary>
    void OnPlayersReady()
    {
        readyPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// This method is called when the song ends, which then shows the player score.
    /// When playing with multiplayer, the scores are compared, and the winner is shown.
    /// </summary>
    void OnGameEnd()
    {
        scoreScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("MainMenuButton"));
        p1score.text = $"Score: {scoreManager.p1Score.score}";
        p1streak.text = $"Final streak: {scoreManager.p1Score.hitStreak}";
        p1hstreak.text = $"Highest streak: {scoreManager.p1Score.highestHitStreak}";
        p1hit.text = $"Notes hit: {scoreManager.p1Score.hitNotes}";
        p1missed.text = $"Notes missed: {scoreManager.p1Score.missedNotes}";

        if (PlayerManager.multiplayer)
        {
            p2ScoreTab.gameObject.SetActive(true);
            p2score.text = $"Score: {scoreManager.p2Score.score}";
            p2streak.text = $"Final streak: {scoreManager.p2Score.hitStreak}";
            p2hstreak.text = $"Highest streak: {scoreManager.p2Score.highestHitStreak}";
            p2hit.text = $"Notes hit: {scoreManager.p2Score.hitNotes}";
            p2missed.text = $"Notes missed: {scoreManager.p2Score.missedNotes}";
            winnerText.gameObject.SetActive(true);
            if (scoreManager.p1Score.score == scoreManager.p2Score.score)
                winnerText.text = "Draw!";
            else
                winnerText.text = scoreManager.p1Score.score > scoreManager.p2Score.score ? "Player 1 wins!" : "Player 2 wins!";
        }
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

    /// <summary>
    /// Activates and updates the ready-up panel for both single- and multiplayer.
    /// </summary>
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


    /// <summary>
    /// Updates the score, the multiplier and the multiplier bar fill for the player(s).
    /// </summary>
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

    /// <summary>
    /// Shows and updates a bar that shows how long the current song lasts.
    /// </summary>
    void SongElements()
    {
        timefill.fillAmount =
            ((float)SongVariables.playbackTime / (AudioManager.songLength / 10)) * 100;
    }

    /// <summary>
    /// Load a scene by name.
    /// Used for loading the main menu scene after finishing the song.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadGameScene(string sceneName) => SceneManager.LoadScene(sceneName);
}