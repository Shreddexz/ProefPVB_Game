using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        p1Score.multiplier = 1;
        p2Score.multiplier = 1;
    }

    void Update()
    {
        UpdateMultiplier();
    }

    void UpdateMultiplier()
    {
        if (p1Score.multiplierStage < multiplierStages[^2] &&
            p1Score.multiplierValue >= multiplierThresholds[p1Score.multiplierStage])
        {
            IncreaseMultiplier(0);
        }

        if (p2Score.multiplierStage < multiplierStages[^2] &&
            p2Score.multiplierValue >= multiplierThresholds[p2Score.multiplierStage])
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
                if (p1Score.hitStreak > p1Score.highestHitStreak)
                    p1Score.highestHitStreak = p1Score.hitStreak;
                p1Score.score += scoreGain * p1Score.multiplier;
                p1Score.multiplierValue += scoreGain * p1Score.multiplier;
                p1Score.hitNotes++;
                break;

            case 1:
                p2Score.hitStreak++;
                if (p2Score.hitStreak > p2Score.highestHitStreak)
                    p2Score.highestHitStreak = p2Score.hitStreak;
                p2Score.score += scoreGain * p2Score.multiplier;
                p2Score.multiplierValue += scoreGain * p2Score.multiplier;
                p2Score.hitNotes++;
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
                if (p1Score.multiplierStage > 0)
                    p1Score.multiplierStage--;

                p1Score.multiplierValue = p1Score.multiplierStage - 1 > 0
                                              ? multiplierThresholds[p1Score.multiplierStage - 1]
                                              : 0;

                p1Score.multiplier = multiplication[p1Score.multiplierStage];
                p1Score.hitStreak = 0;
                p1Score.missedNotes++;
                break;

            case 1:
                if (p2Score.multiplierStage > 0)
                    p2Score.multiplierStage--;

                p2Score.multiplierValue = p2Score.multiplierStage - 1 > 0
                                              ? multiplierThresholds[p2Score.multiplierStage - 1]
                                              : 0;

                p2Score.multiplier = multiplication[p2Score.multiplierStage];
                p2Score.hitStreak = 0;
                p2Score.missedNotes++;
                break;
        }
    }


    public struct ScoreInstance
    {
        public int score;
        public int hitStreak;
        public int highestHitStreak;
        public int multiplier;
        public int multiplierValue;
        public int multiplierStage;
        public int missedNotes;
        public int hitNotes;
    }
}