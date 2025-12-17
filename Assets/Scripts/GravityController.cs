using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public GameObject panel;
    public float gravityStrength = 9.8f;
    public GameObject start;

    [Header("Wind Effects")]
    public ParticleSystem windUp;
    public ParticleSystem windDown;
    public ParticleSystem windLeft;
    public ParticleSystem windRight;

    public float windDuration = 2f;
    private Coroutine windRoutine;

    void Start()
    {
        if (start != null)
            start.SetActive(true);

        Physics2D.gravity = Vector2.zero;
    }

    void Update()
    {
        if (panel != null && panel.activeSelf) return;
        if (Time.timeScale == 0) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;

            float w = Screen.width;
            float h = Screen.height;


            if (pos.x < w * 0.33f)
            {
                SetGravity(Vector2.left, windLeft);
            }
            else if (pos.x > w * 0.66f)
            {
                SetGravity(Vector2.right, windRight);
            }
            else if (pos.y > h * 0.66f)
            {
                SetGravity(Vector2.up, windUp);
            }
            else if (pos.y < h * 0.33f)
            {
                SetGravity(Vector2.down, windDown);
            }
        }
        
    }

    void SetGravity(Vector2 direction, ParticleSystem windEffect)
    {
        Physics2D.gravity = direction * gravityStrength;

        if (start != null)
            start.SetActive(false);

        if (windEffect == null) return;

        windEffect.Play();

        if (windRoutine != null)
            StopCoroutine(windRoutine);

        windRoutine = StartCoroutine(StopWindAfterSeconds(windEffect, windDuration));
    }

    IEnumerator StopWindAfterSeconds(ParticleSystem ps, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (ps != null)
            ps.Stop();

        windRoutine = null;
    }
}
