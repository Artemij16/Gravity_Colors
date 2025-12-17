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
        if (triggered) return;
        if (!col.CompareTag("Player")) return;

        triggered = true;

        // записываем выбранный режим перед загрузкой
        switch (mode)
        {
            case ModeType.Normal:
                GameModeManager.CurrentMode = GameMode.Normal;
                break;
            case ModeType.Timer:
                GameModeManager.CurrentMode = GameMode.Timer;
                break;
            case ModeType.Health:
                GameModeManager.CurrentMode = GameMode.Health;
                break;
            case ModeType.Tasks:
                GameModeManager.CurrentMode = GameMode.Tasks;
                break;
        }

        Vector3 targetPos = transform.position;

        cameraZoom.StartZoom(targetPos, () =>
        {
            SceneManager.LoadScene(levelName);
        });
    }
}
