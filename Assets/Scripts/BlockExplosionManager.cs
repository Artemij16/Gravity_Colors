using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockExplosionManager : MonoBehaviour
{
    public static BlockExplosionManager Instance;

    [Header("Prefab эффекта взрыва")]
    public GameObject explosionPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CreateExplosion(Vector3 position, Color color)
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning("⚠️ Explosion prefab не назначен в инспекторе!");
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        var ps = explosion.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = color;
        }

        Destroy(explosion, 2f);
    }
}
