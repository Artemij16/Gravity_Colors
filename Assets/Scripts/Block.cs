using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Color blockColor;
    SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

    }
}
