using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Это "адрес", по которому другие скрипты найдут этот объект
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Эта функция — как приемник для кассет. Кто бы её ни вызвал, она сыграет звук.
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}