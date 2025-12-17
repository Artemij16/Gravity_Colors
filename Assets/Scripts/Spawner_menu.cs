using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_menu : MonoBehaviour
{
    [Header("Spawner movement")]
    public float leftX = -5f;
    public float rightX = 5f;
    public float moveSpeed = 2f;

    [Header("Spawn settings")]
    public GameObject[] prefabs;
    public float spawnInterwal = 1f;

    private float timer;
    private int direction = 1;

    private void Update()
    {
        MoveSpawner();
        HandleSpawning();
    }

    void MoveSpawner()
    {
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        if (transform.position.x >= rightX)
            direction = -1;
        if (transform.position.x <= leftX)
            direction = 1;

    }
    void HandleSpawning()
    {
        timer += Time.deltaTime;

        if(timer > spawnInterwal)
        {
            SpawnRandomObject();
            timer = 0;
        }
    }
    void SpawnRandomObject()
    {
        if (prefabs.Length == 0)
            return;
        int index = Random.Range(0, prefabs.Length);

        float randomZ = Random.Range(0, 360f);
        Quaternion rotation = Quaternion.Euler(0, 0, randomZ);
        Instantiate(prefabs[index], transform.position, rotation);
    }
}
