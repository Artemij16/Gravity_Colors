using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedManager : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject ded;
    public float spawnInterval = 20f;
    public float spawnerCooldown = 3f;

    float timer;



    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            spawnInterval -= 0.3f; 
            SpawnDed();
        }
    }

    private void SpawnDed()
    {

        int attempts = 0;
        int index = Random.Range(0, spawners.Length);

        while (!spawners[index].activeInHierarchy && attempts < 10)
        {
            index = Random.Range(0, spawners.Length);
            attempts++;
        }

        if (!spawners[index].activeInHierarchy)
        {
            return;
        }

        Transform spawnPoint = spawners[index].transform;
        Instantiate(ded, spawnPoint.position, Quaternion.identity);

        StartCoroutine(DisableSpawner(spawners[index]));
    }

    private IEnumerator DisableSpawner(GameObject spawner)
    {
        spawner.SetActive(false);
        yield return new WaitForSeconds(spawnerCooldown);
        spawner.SetActive(true);
    }
}
