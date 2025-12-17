using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public AudioMixer audioMixer;

    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public bool vibrationEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings();
        ApplySettings();
    }

    public void SetSoundVolume(float value)
    {
        soundVolume = value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SoundVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetVibration(bool enabled)
    {
        vibrationEnabled = enabled;
        PlayerPrefs.SetInt("Vibration", enabled ? 1 : 0);
    }

    private void LoadSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        vibrationEnabled = PlayerPrefs.GetInt("Vibration", 1) == 1;
    }

    private void ApplySettings()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(soundVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
    }
}
