using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [NonSerialized] public ScoreInstance p1Score;
    [NonSerialized] public ScoreInstance p2Score;
    public int[] multiplierStages;
    public int[] multiplication;
    public int[] multiplierThresholds;

    public int perfectScore = 50, goodScore = 20, okScore = 10;

    void Awake()
    {
        p1Score = new();
    }

    void Update()
    {
        UpdateMultiplier();
    }

    void UpdateMultiplier()
    {
        if (p1Score.multiplierValue >= multiplierThresholds[p1Score.multiplierStage])
        {
            IncreaseMultiplier(0);
        }

        if (p2Score.multiplierValue >= multiplierThresholds[p2Score.multiplierStage])
        {
            IncreaseMultiplier(1);
        }
    }

    public void AddScore(int id, string hitType)
    {
        int scoreGain;

        switch (hitType)
        {
            case "Perfect":
                scoreGain = perfectScore;
                break;
            case "Good":
                scoreGain = goodScore;
                break;
            case "OK":
                scoreGain = okScore;
                break;

            default:
                scoreGain = okScore;
                break;
        }

        switch (id)
        {
            case 0:
                p1Score.hitStreak++;
                p1Score.score += scoreGain * p1Score.multiplier;
                p1Score.multiplierValue += scoreGain * p1Score.multiplier;
                break;

            case 1:
                p2Score.hitStreak++;
                p2Score.score += scoreGain * p2Score.multiplier;
                p2Score.multiplierValue += scoreGain * p2Score.multiplier;
                break;
        }
    }

    public void AddPlayer()
    {
        p2Score = new();
        p2Score.multiplier = 1;
    }

    public void IncreaseMultiplier(int id)
    {
        switch (id)
        {
            case 0:
                p1Score.multiplierStage++;
                p1Score.multiplier = multiplication[p1Score.multiplierStage];
                break;
            case 1:
                p2Score.multiplierStage++;
                p2Score.multiplier = multiplication[p2Score.multiplierStage];
                break;
        }
    }

    public void ReduceMultiplier(int id)
    {
        switch (id)
        {
            case 0:
                p1Score.multiplierStage--;
                p1Score.multiplier = multiplication[p1Score.multiplierStage];
                p1Score.multiplierValue = multiplierStages[p1Score.multiplierStage];
                p1Score.hitStreak = 0;
                break;
            case 1:
                p2Score.multiplierStage--;
                p2Score.multiplier = multiplication[p2Score.multiplierStage];
                p2Score.multiplierValue = multiplierStages[p2Score.multiplierStage];
                p2Score.hitStreak = 0;
                break;
        }
    }


    public struct ScoreInstance
    {
        public int score;
        public int hitStreak;
        public int multiplier;
        public int multiplierValue;
        public int multiplierStage;
    }
}