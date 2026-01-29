using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public bool canSpawn = true;

    [Header("Prefabs")]
    public GameObject[] blockPrefabs;
    public GameObject bonusBlockPrefab;
    public GameObject freezeBlockPrefab;
    public GameObject randomBlockPrefab;

    [Header("Settings spawn")]
    public float spawnInterval = 2f;
    public float minSpawnInterval = 1f;

    private float timer;
    private int spawnCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!canSpawn) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        GameObject prefabToSpawn;


        float chance = Random.value;

        if (chance < 0.05f && bonusBlockPrefab != null)
        {
            prefabToSpawn = bonusBlockPrefab;
        }
        else if (chance < 0.25f && freezeBlockPrefab != null)
        {
            prefabToSpawn = freezeBlockPrefab;
        }
        else if (chance < 0.15f && randomBlockPrefab != null)
        {
            prefabToSpawn = randomBlockPrefab;
        }
        else
        {
            int index = Random.Range(0, blockPrefabs.Length);
            prefabToSpawn = blockPrefabs[index];
        }

        float randomRotation = Random.Range(0f, 360f);
        Instantiate(prefabToSpawn, transform.position, Quaternion.Euler(0, 0, randomRotation));


        spawnCount++;
        if (spawnCount >= 5)
        {
            spawnCount = 0;
            if (spawnInterval > minSpawnInterval)
            {
                spawnInterval -= 0.1f;
                if (spawnInterval < minSpawnInterval) spawnInterval = minSpawnInterval;
            }
        }
    }
}
