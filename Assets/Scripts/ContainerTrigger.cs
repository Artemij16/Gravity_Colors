using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContainerTrigger : MonoBehaviour
{
    public enum ModeType
    {
        Normal,
        Timer,
        Health,
        Tasks
    }

    [Header("Что загружаем")]
    public string levelName;

    [Header("Какой режим запускается при входе")]
    public ModeType mode = ModeType.Normal;

    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private GameObject grav;

    private bool triggered = false;

    public void Update()
    {
        if (Physics2D.gravity.y != 0 || Physics2D.gravity.x != 0)
            grav.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered || !col.CompareTag("Player")) return;
        triggered = true;

        GameModeManager.CurrentMode = (GameMode)mode;

        PlayerPrefs.SetString("SavedGameMode", GameModeManager.CurrentMode.ToString());
        PlayerPrefs.Save();

        cameraZoom.StartZoom(transform.position, () => {
            SceneManager.LoadScene(levelName);
        });
    }
}
