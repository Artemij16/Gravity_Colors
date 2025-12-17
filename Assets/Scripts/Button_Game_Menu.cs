using UnityEngine;

public class Button_Game_Menu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OnButtonClick()
    {
        if (panel == null)
        {
            Debug.LogError("⚠️ Панель не назначена в инспекторе!");
            return;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;
    }
}
