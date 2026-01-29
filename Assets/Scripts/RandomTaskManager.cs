using UnityEngine;
using TMPro;

public class RandomTaskManager : MonoBehaviour
{
    public static RandomTaskManager Instance;

    [Header("UI")]
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI timerText;

    [Header("Task settings")]
    public int minCount = 5;
    public int maxCount = 12;

    [Header("Timer Settings")]
    public float startDuration = 180f;
    public float timeReduction = 10f;
    public float minAllowedTime = 20f;

    private float currentTimeLimit;
    private float timeRemaining;
    private bool isTimerRunning = false;

    string[] colors = { "Red", "Blue", "Green", "Yellow" };

    private string currentColor;
    private int targetCount;
    private int currentCount;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTimeLimit = startDuration;
        GenerateNewTask();
        isTimerRunning = true;
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            isTimerRunning = false;
            GameOverByTimer();
        }
    }

    public void GenerateNewTask()
    {
        currentColor = colors[Random.Range(0, colors.Length)];
        targetCount = Random.Range(minCount, maxCount);
        currentCount = 0;

        timeRemaining = currentTimeLimit;

        UpdateUI();
    }

    public void AddProgress(string colorTag)
    {
        if (colorTag == currentColor)
        {
            currentCount++;

            if (currentCount >= targetCount)
            {
                ScoreManager.Instance.AddScore(50);
                currentTimeLimit -= timeReduction;
                if (currentTimeLimit < minAllowedTime) currentTimeLimit = minAllowedTime;

                GenerateNewTask();
            }
            else
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        taskText.text = $"Collect: {targetCount} {currentColor}";
        progressText.text = $"{currentCount} / {targetCount}";
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOverByTimer()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.GameOver();
        }
    }
}

