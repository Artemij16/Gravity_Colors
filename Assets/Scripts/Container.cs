using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Container : MonoBehaviour
{
    public string istag = "";
    public int scoreValue = 10;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private HealthManager healthManager;
    private Timer_Game timer;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(FlashTransparency());

        Color explosionColor = GetColorByTag(other.tag);

        if (BlockExplosionManager.Instance != null)
            BlockExplosionManager.Instance.CreateExplosion(other.transform.position, explosionColor);

        if (other.tag == istag)
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(scoreValue);

            if (RandomTaskManager.Instance != null)
                RandomTaskManager.Instance.AddProgress(other.tag);

            Destroy(other.gameObject);

            ShakeCameraSafe(0.08f, 0.08f);
        }
        else
        {
            // режим с заданиями (tasks)
            if (GameModeManager.CurrentMode == GameMode.Tasks)
            {
                ScoreManager.Instance.MinusScore(20);
                ShakeCameraSafe(0.2f, 0.3f);

                Destroy(other.gameObject);
                return;
            }

            // режим жизней
            if (GameModeManager.CurrentMode == GameMode.Health)
            {
                if (healthManager.Health > 1)
                    ShakeCameraSafe(0.2f, 0.3f);

                healthManager.MinusHealth();
                Destroy(other.gameObject);
                return;
            }

            // режим таймера
            if (GameModeManager.CurrentMode == GameMode.Timer)
            {
                if (timer.timeRemaining > 0)
                    ShakeCameraSafe(0.2f, 0.3f);

                timer.MinusTime();
                Destroy(other.gameObject);
                return;
            }

            // NORMAL mode (ничего не делаем)
            Destroy(other.gameObject);
        }



    }

    void ShakeCameraSafe(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        CameraShake shake = cam.GetComponent<CameraShake>();
        if (shake != null)
            StartCoroutine(shake.Shake(duration, magnitude));
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
