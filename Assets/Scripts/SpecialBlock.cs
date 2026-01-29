using UnityEngine;
using System.Collections;

public class SpecialBlock : MonoBehaviour
{
    public bool isRandomBlock = false;
    private SpriteRenderer sr;
    private string[] colors = { "Red", "Blue", "Green", "Yellow" };

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isRandomBlock)
        {
            StartCoroutine(RandomizeRoutine());
        }
    }

    IEnumerator RandomizeRoutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            SetRandomState();
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.05f;
        }
        SetRandomState();
    }

    void SetRandomState()
    {
        int rand = Random.Range(0, colors.Length);
        tag = colors[rand];
        sr.color = GetColorByTag(tag);
    }

    Color GetColorByTag(string t)
    {
        if (t == "Red") return Color.red;
        if (t == "Blue") return Color.blue;
        if (t == "Green") return Color.green;
        if (t == "Yellow") return Color.yellow;
        return Color.white;
    }
}