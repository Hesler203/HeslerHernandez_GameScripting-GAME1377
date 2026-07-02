using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private float spawnDelay = 2.0f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private const float SPAWN_X_POS = 30f;
    [SerializeField] private float spawnYMin = 3f;
    [SerializeField] private float spawnYMax = 15f;

    void Start()
    {
        InvokeRepeating("SpawnObjects", spawnDelay, spawnInterval);
    }

    /// <summary>
    /// Spawns objects from the objectPrefabs[] within the spawn range while game is active
    /// </summary>
    void SpawnObjects()
    {
        // Set random spawn location and random object index
        float spawnYRange = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnLocation = new Vector3(SPAWN_X_POS, spawnYRange);
        int randomIndex = Random.Range(0, objectPrefabs.Length);

        // While game is active, spawn new object
        if (!GameManager.IsGameOver)
        {
            Instantiate(objectPrefabs[randomIndex], spawnLocation, objectPrefabs[randomIndex].transform.rotation);
        }
    }
}
