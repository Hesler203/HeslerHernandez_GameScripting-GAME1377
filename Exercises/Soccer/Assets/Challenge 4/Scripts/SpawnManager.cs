using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;

    [Header("Spawn Bounds")]
    [SerializeField] private float spawnRangeX = 10;
    [SerializeField] private float spawnZMin = 15;
    [SerializeField] private float spawnZMax = 25;

    [Header("Counters")]
    [SerializeField] private int waveCount = 1;
    [SerializeField] private int enemyCount = 0;

    [Header("Player-related References")]
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject focalPoint;
    [SerializeField] public Transform playerGoal;

    void Update()
    {
        if (enemyCount == 0)
        {
            SpawnEnemyWave();

        }
    }

    /// <summary>
    /// Spawns an amount of enemies matching the current wave count as children of the spawn manager,
    /// initilializing their reference to this script, increasing their speed as waves progress, and
    /// increasing the current enemy count after each spawn.
    /// Then, increases the current wave count after the last spawn.
    /// </summary>
    private void SpawnEnemyWave()
    {
        for (int i = 0; i < waveCount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            spawnedEnemy.GetComponent<Enemy>().SetSpawnerRef(this);
            enemyCount++;
        }
        waveCount++;
    }

    /// <summary>
    /// Generates & returns a random position within the spawn range bounds at which to spawn at.
    /// </summary>
    /// <returns>A Vector3 representing the position to spawn at.</returns>
    private Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }
}
