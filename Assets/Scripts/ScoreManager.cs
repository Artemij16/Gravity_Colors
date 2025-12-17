using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Text scoreText;
    public GameObject gameOverPanel;
    public static int score = 2000;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void MinusScore(int value)
    {
        score -= value;

        if (score < 0)
            score = 0;

        scoreText.text = "Score: " + score;
    }

}
