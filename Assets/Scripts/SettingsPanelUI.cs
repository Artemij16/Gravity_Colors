using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;
    public Toggle vibrationToggle;

    private void OnEnable()
    {
        if (SettingsManager.Instance == null) return;

        soundSlider.value = SettingsManager.Instance.soundVolume;
        musicSlider.value = SettingsManager.Instance.musicVolume;

        if (vibrationToggle != null)
            vibrationToggle.isOn = SettingsManager.Instance.vibrationEnabled;
    }

    public void OnSoundChanged(float value)
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetSoundVolume(value);
    }

    public void OnMusicChanged(float value)
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetMusicVolume(value);
    }

    public void OnVibrationChanged(bool enabled)
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetVibration(enabled);
    }

    public void SaveAndClose()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetSoundVolume(soundSlider.value);
            SettingsManager.Instance.SetMusicVolume(musicSlider.value);

            if (vibrationToggle != null)
                SettingsManager.Instance.SetVibration(vibrationToggle.isOn);

            PlayerPrefs.Save();
        }

        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
