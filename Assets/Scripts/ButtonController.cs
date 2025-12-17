using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [Header("Scene Names")]
    public string playScene = "Game";
    public string shopScene = "Shop";
    public GameObject panel;
    public GameObject panel_about;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void OnPlayButton()
    {
        SceneManager.LoadScene(playScene);
    }

    public void OnShopButton()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void OnSettingsButton()
    {
        panel.SetActive(true);
    }

    public void OnAboutButton()
    {
        panel_about.SetActive(true);
    }

    public void OnExitButton()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }
    public void OnButtonExitFromAbout()
    {
        panel_about.SetActive(false);
    }
}
