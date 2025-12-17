using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Game : MonoBehaviour
{
    public static Timer_Game instance;

    public Text timerText;
    public float timeRemaining = 300f;
    private bool timerRunning = true;
    public GameObject panel_res;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
            TimerFinished();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        if (panel_res != null)
            panel_res.SetActive(true);

        Time.timeScale = 0f;
    }
    public void MinusTime()
    {
        timeRemaining -= 10f;
        if (timeRemaining < 0f) timeRemaining = 0f;
    }

    public void PauseTimer() => timerRunning = false;
    public void ResumeTimer() => timerRunning = true;
}
