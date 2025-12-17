using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    public Text healthText;

    public int Health = 5;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (healthText != null)
            healthText.text = "" + Health;

        Time.timeScale = 1.0f;
    }

    public void MinusHealth()
    {
        Health--;
        if (healthText != null)
            healthText.text = "" + Health;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            ScoreManager.Instance.GameOver();
        }
    }
}
