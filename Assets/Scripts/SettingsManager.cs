using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
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
            LoadSettings();
            StartCoroutine(ApplyVolumesNextFrame());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void TriggerVibration()
    {
        if (vibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
    private IEnumerator ApplyVolumesNextFrame()
    {

        yield return new WaitForEndOfFrame();
        ApplyVolumes();
    }

    public void SetSoundVolume(float value)
    {
        soundVolume = value;
        audioMixer.SetFloat("SFXVolume", LinearToDb(value));
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
    private void ApplyVolumes()
    {
        SetMusicVolume(musicVolume);
        SetSoundVolume(soundVolume);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat("MusicVolume", LinearToDb(value));
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

    private float LinearToDb(float value)
    {
        if (value <= 0.0001f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }
}
