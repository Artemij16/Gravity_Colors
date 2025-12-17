using UnityEngine;
using TMPro;

public class RandomTaskManager : MonoBehaviour
{
    public static RandomTaskManager Instance;

    [Header("UI")]
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI progressText;

    [Header("Task settings")]
    public int minCount = 5;
    public int maxCount = 12;

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
        GenerateNewTask();
    }

    public void GenerateNewTask()
    {
        currentColor = colors[Random.Range(0, colors.Length)];
        targetCount = Random.Range(minCount, maxCount);
        currentCount = 0;

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
}
