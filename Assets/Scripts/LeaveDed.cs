using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveDed : MonoBehaviour
{
    public float spawnInterval = 1f;
    public float liveInterval = 3f;
    public float growSpeed = 2f;

    private float timerspawn = 0f;
    private float timerlive = 0f;
    private Vector3 targetScale = Vector3.one;

    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        timerspawn += Time.deltaTime;
        timerlive += Time.deltaTime;


        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, growSpeed * Time.deltaTime);

        if (timerlive >= liveInterval)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Color explosionColor = GetColorByTag(other.tag);
    BlockExplosionManager.Instance.CreateExplosion(other.transform.position, explosionColor);
    
    switch (GameModeManager.CurrentMode)
        {
            case GameMode.Health:
                if (HealthManager.Instance != null)
                {
                    if (HealthManager.Instance.Health > 1)
                        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.2f, 0.3f));
                HealthManager.Instance.MinusHealth();
            }
                break;

            case GameMode.Timer:
                if (Timer_Game.instance != null)
                {
                    if (Timer_Game.instance.timeRemaining > 0)
                        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.2f, 0.3f));
                Timer_Game.instance.MinusTime();
            }
                break;

            case GameMode.Tasks:
                StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.2f, 0.3f));
                ScoreManager.Instance.MinusScore(20);
                break;

            case GameMode.Normal:
                break;
        }
        Destroy(other.gameObject);
}
    Color GetColorByTag(string tag)
    {
        switch (tag)
        {
            case "Red": return Color.red;
            case "Blue": return Color.blue;
            case "Green": return Color.green;
            case "Yellow": return Color.yellow;
        }
        return Color.white;
    }
}
