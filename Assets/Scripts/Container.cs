using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public string istag = "";
    public int scoreValue = 5;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private HealthManager healthManager;
    private Timer_Game timer;

    [Header("Sounds")]
    public AudioClip explosionSound;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        healthManager = HealthManager.Instance;
        timer = Timer_Game.instance;

        if (healthManager == null) healthManager = FindObjectOfType<HealthManager>();
        if (timer == null) timer = FindObjectOfType<Timer_Game>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.Instance.PlaySFX(explosionSound);
        TriggerVisuals(other);

        if (other.CompareTag("Bonus"))
        {
            ScoreManager.Instance.AddScore(20);
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Freeze"))
        {
            StartCoroutine(SlowMotionRoutine(3f, 0.5f));
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag(istag))
        {

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(scoreValue);

            if (RandomTaskManager.Instance != null)
                RandomTaskManager.Instance.AddProgress(other.tag);

            Destroy(other.gameObject);
        }
        else
        {
            HandleWrongBlock(other);
        }
    }
    IEnumerator SlowMotionRoutine(float duration, float targetScale)
    {
        Time.timeScale = targetScale;
        yield return new WaitForSeconds(duration * targetScale);

        if (Time.timeScale > 0)
        {
            Time.timeScale = 1.0f;
        }
    }

    private void HandleWrongBlock(Collider2D other)
    {
        if (GameModeManager.CurrentMode == GameMode.Tasks)
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.MinusScore(20);
        }
        else if (GameModeManager.CurrentMode == GameMode.Health)
        {
            if (healthManager != null)
            {
                healthManager.MinusHealth();
            }
            else
            {
                healthManager = HealthManager.Instance;
                if (healthManager != null) healthManager.MinusHealth();
            }
        }
        else if (GameModeManager.CurrentMode == GameMode.Timer)
        {
            if (timer != null)
            {
                timer.MinusTime();
            }
            else
            {
                timer = Timer_Game.instance;
                if (timer != null) timer.MinusTime();
            }
        }

        if (other != null && other.gameObject != null)
        {
            Destroy(other.gameObject);
        }
    }
    void TriggerVisuals(Collider2D other)
    {
        
        StartCoroutine(FlashTransparency());

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.TriggerVibration();
        }

        Color explosionColor;

        if (other.CompareTag("Freeze"))
        {

            explosionColor = new Color(0.5f, 0.8f, 1f);
            ShakeCameraSafe(0.15f, 0.15f);
        }
        else if (other.CompareTag("Bonus"))
        {

            explosionColor = new Color(1f, 0.5f, 0f);
            ShakeCameraSafe(0.15f, 0.12f);
        }
        else
        {
            explosionColor = GetColorByTag(other.tag);

            float shakeMag = (other.CompareTag(istag)) ? 0.08f : 0.2f;
            ShakeCameraSafe(0.1f, shakeMag);
        }

        if (BlockExplosionManager.Instance != null)
            BlockExplosionManager.Instance.CreateExplosion(other.transform.position, explosionColor);
    }
    void ShakeCameraSafe(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        CameraShake shake = cam.GetComponent<CameraShake>();
        if (shake != null) StartCoroutine(shake.Shake(duration, magnitude));
    }

    IEnumerator FlashTransparency()
    {
        if (spriteRenderer == null) yield break;
        Color semiTransparent = originalColor;
        semiTransparent.a = 0.5f;
        spriteRenderer.color = semiTransparent;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = originalColor;
    }

    Color GetColorByTag(string tag)
    {
        switch (tag)
        {
            case "Red": return Color.red;
            case "Blue": return Color.blue;
            case "Green": return Color.green;
            case "Yellow": return Color.yellow;
            default: return Color.white;
        }
    }
}
