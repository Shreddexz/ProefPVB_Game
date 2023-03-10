using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    ScoreInstance p1Score;
    ScoreInstance p2Score;

    public int perfectScore = 50, goodScore = 20, okScore = 10;

    void Awake()
    {
        p2Score = new();
    }


    public void AddScore(int ID, string hitType)
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

        switch (ID)
        {
            case 0:
                p1Score.score += scoreGain * p1Score.multiplier;
                Debug.Log($"Player one has a score of {p1Score.score} at {Time.unscaledDeltaTime}");
                break;
            case 1:
                p2Score.score += scoreGain * p2Score.multiplier;
                Debug.Log($"Player two has a score of {p2Score.score} at {Time.unscaledDeltaTime}");
                break;
        }
    }

    public void AddPlayer()
    {
        p2Score = new();
    }

    public struct ScoreInstance
    {
        public int score;
        public int hitStreak;
        public int multiplier;
    }
}