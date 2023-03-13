using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI multiValue;

    void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }

    void Update()
    {
        PlayerScore();
    }

    void PlayerScore()
    {
        scoreText.text = scoreManager.p1Score.score.ToString();
        multiText.text = $"{scoreManager.p1Score.multiplier.ToString()}x";
        multiValue.text = scoreManager.p1Score.multiplierValue.ToString();
    }
}