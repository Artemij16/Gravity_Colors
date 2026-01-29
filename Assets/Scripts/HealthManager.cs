using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    public Text healthText;


    private bool isDead = false;
    public int Health = 5;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (healthText != null)
            healthText.text = "" + Health;
    }

    public void MinusHealth()
    {
        Health--;
        if (healthText != null)
            healthText.text = "" + Health;
    }

    private void Update()
    {
        if (Health <= 0 && !isDead)
        {
            isDead = true;
            ScoreManager.Instance.GameOver();
        }
    }
}
