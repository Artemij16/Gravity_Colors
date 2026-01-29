using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartButton : MonoBehaviour
{
    [Header("Scene Names")]
    public string menuScene = "MainMenu";

    public TextMeshProUGUI score_;


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Physics2D.gravity = Vector2.down * 9.8f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
