using UnityEngine;
using UnityEngine.UI;

public class BackgroundItem : MonoBehaviour
{
    public Image preview;
    public GameObject lockIcon;

    public void Set(Sprite sprite, bool locked)
    {
        preview.sprite = sprite;
        lockIcon.SetActive(locked);
    }
}
