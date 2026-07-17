using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size.
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    private float playerSafeDistance = 3f;
    private Vector3 randomSpawnLocation;
    private float spawnDistance;

    [Header("Settings")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private int initalSpawnAmount = 5;
    [SerializeField] public int childSpawnAmount = 2;

    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;

        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;

        SpawnInitialAsteroids();
    }

    /// <summary>
    /// Spawns the initial large asteroids at random locations within a safe distance from player.
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < initalSpawnAmount; i++)
        {
            do
            {
                SetRandomSpawnLocation();
            } while (spawnDistance < playerSafeDistance);

            SpawnAsteroid(randomSpawnLocation, Asteroid.AsteroidSize.Large, initalSpawnAmount / initalSpawnAmount);
        }
    }

    /// <summary>
    /// Sets the randomSpawnLocation to be a new Vector3 using random ranges within the screen bounds,
    /// and the spawnDistance from the center of the screen to the randomSpawnLocation.
    /// </summary>
    private void SetRandomSpawnLocation()
    {
        float randomXInBounds = Random.Range(spawnXMin, spawnXMax);
        float randomYInBounds = Random.Range(spawnYMin, spawnYMax);
        randomSpawnLocation = new Vector3(randomXInBounds, randomYInBounds);
        spawnDistance = Vector3.Distance(Vector3.zero, randomSpawnLocation);
    }

    /// <summary>
    /// Spawns a spawnAmount number of asteroids at the passed in position of the size determined
    /// by the passed in Asteroid size enum & initializes their reference to this spawner before
    /// making them childs of the spawner to tidy scene hierarchy.
    /// </summary>
    /// <param name="position">Vector3 position to spawn at.</param>
    /// <param name="size">The enum asteroid size that determines which asteroid prefab to spawn.</param>
    /// <param name="spawnAmount">The number of asteroids to spawn.</param>
    ///
    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size, int spawnAmount)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject asteroidToSpawn = null;
            foreach (GameObject asteroidPrefab in asteroidPrefabs)
            {
                if (asteroidPrefab.GetComponent<Asteroid>().size == size)
                {
                    asteroidToSpawn = asteroidPrefab;
                }
            }

            if (asteroidToSpawn != null)
            {
                GameObject spawnedAsteroid  = Instantiate(asteroidToSpawn, position, asteroidToSpawn.transform.rotation);
                spawnedAsteroid.GetComponent<Asteroid>().InitializeSpawnerRef(this);
                spawnedAsteroid.transform.parent = transform;
            }
            else
            {
                return;
            }
        }
    }
}