using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultScoreText;
    public TextMeshProUGUI highScoreText;
    public Image x2ButtonImage;

    [Header("Objects")]
    public GameObject gameOverPanel;
    public GameObject x2Button;

    [Header("Settings")]
    public float timerDuration = 5f;

    public int score = 0;
    private bool isGameOver = false;
    private bool x2Used = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        if (PlayerPrefs.HasKey("SavedGameMode"))
        {
            string modeName = PlayerPrefs.GetString("SavedGameMode");
            System.Enum.TryParse(modeName, out GameModeManager.CurrentMode);
        }
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void MinusScore(int value)
    {
        score -= value;
        if (score < 0) score = 0;
        UpdateUI();
    }

    private void UpdateUI() => scoreText.text = "Score: " + score;

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (x2Button != null)
        {
            x2Button.SetActive(true);
            StartX2Timer();
        }

        if (Spawner.Instance != null) Spawner.Instance.canSpawn = false;

        UpdateBank(score);

        string modeKey = "Highscore_" + GameModeManager.CurrentMode.ToString();
        int best = DataCrypto.GetSecureInt(modeKey, 0);

        if (score > best)
        {
            DataCrypto.SaveSecureInt(modeKey, score);
            best = score;
            highScoreText.text = "NEW BEST: " + best;
        }
        else
        {
            highScoreText.text = "BEST: " + best;
        }

        resultScoreText.text = "SCORE: " + score;
        x2Used = false;
        PlayerPrefs.Save();
        gameOverPanel.SetActive(true);
    }

    public void RewardDoubleScore()
    {
        if (x2Used) return;
        x2Used = true;
        StopAllCoroutines();

        UpdateBank(score);
        resultScoreText.text = "SCORE: " + (score * 2);

        if (x2Button != null) x2Button.SetActive(false);
        PlayerPrefs.Save();
    }

    private void UpdateBank(int amountToAdd)
    {
        int currentBank = DataCrypto.GetSecureInt("TotalBank", 0);
        currentBank += amountToAdd;
        DataCrypto.SaveSecureInt("TotalBank", currentBank);
    }

    public void StartX2Timer() => StartCoroutine(ButtonTimerCoroutine());

    IEnumerator ButtonTimerCoroutine()
    {
        float elapsedTime = 0;
        x2ButtonImage.fillAmount = 1;

        while (elapsedTime < timerDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            x2ButtonImage.fillAmount = 1f - (elapsedTime / timerDuration);
            yield return null;
        }

        x2Button.SetActive(false);
    }
}