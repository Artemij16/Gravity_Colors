using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    public SpriteRenderer bgRenderer;
    public Sprite[] backgrounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyBackground();
    }

    public void ApplyBackground()
    {
        int index = PlayerPrefs.GetInt("SelectedBG", 0);

        // Если ссылка на рендерер пустая (после смены сцены), ищем его по тегу или имени
        if (bgRenderer == null)
        {
            GameObject bgObj = GameObject.FindWithTag("MainBackground"); // Дай фону в сценах тег "MainBackground"
            if (bgObj != null) bgRenderer = bgObj.GetComponent<SpriteRenderer>();
        }

        if (bgRenderer != null && index < backgrounds.Length)
        {
            bgRenderer.sprite = backgrounds[index];
        }
    }

    public void SetBackground(int index)
    {
        PlayerPrefs.SetInt("SelectedBG", index);
        ApplyBackground();
    }
}
