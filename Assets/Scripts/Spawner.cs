using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public float spawnInterval = 2f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        int index = Random.Range(0, blockPrefabs.Length);

        float randomRotation = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0, 0, randomRotation);

        Instantiate(blockPrefabs[index], transform.position, rotation);
    }
}
