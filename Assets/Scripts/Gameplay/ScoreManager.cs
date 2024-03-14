using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public TextMeshProUGUI scoreText;

    void Start() {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void UpdateScore(int newScore) {
        currentScore = newScore;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay() {
        if (scoreText != null) {
            scoreText.text = currentScore.ToString();
        }
    }
}
