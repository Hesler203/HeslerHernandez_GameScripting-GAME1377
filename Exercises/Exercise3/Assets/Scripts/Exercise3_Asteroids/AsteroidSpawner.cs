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

    [SerializeField] private GameObject asteroidSmall;
    [SerializeField] private GameObject asteroidMedium;
    [SerializeField] private GameObject asteroidLarge;

    [SerializeField] private int initalSpawnAmount;
    [SerializeField] private int childSpawnAmount = 2;

    private float randomXInBounds;
    private float randomYInBounds;
    private Vector3 randomSpawnLocation;

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
    /// Spawns the initial large asteroids at random locations within safe distance from player
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < initalSpawnAmount; i++)
        {
            SetRandomSpawnLocation();
            if (IsSpawnDistanceSafe())
            {
                SpawnAsteroid(randomSpawnLocation, Asteroid.AsteroidSize.Large);
            }
        }
    }

    /// <summary>
    /// Sets the randomSpawnLocation to be a new Vector3 using random ranges within the screen bounds
    /// </summary>
    private void SetRandomSpawnLocation()
    {
        randomXInBounds = Random.Range(spawnXMin, spawnXMax);
        randomYInBounds = Random.Range(spawnYMin, spawnYMax);
        randomSpawnLocation = new Vector3(randomXInBounds, randomYInBounds);
    }

    /// <summary>
    /// Continuously sets a new RandomSpawnLocation and checks for it being within safe distance of the player
    /// </summary>
    /// <returns>true once the potential asteroid spawn location is past the playerSafeDistance</returns>
    private bool IsSpawnDistanceSafe()
    {
        float spawnDistance = Vector3.Distance(Vector3.zero, randomSpawnLocation);

        while (spawnDistance < playerSafeDistance)
        {
            SetRandomSpawnLocation();
            spawnDistance = Vector3.Distance(Vector3.zero, randomSpawnLocation);
        }
        return true;
    }

    /// <summary>
    /// Spawns asteroids based on the passed in size enum & initializes their reference to this spawner
    /// </summary>
    /// <param name="position">the spawning location</param>
    /// <param name="size">determines which asteroid prefab to spawn</param>
    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size)
    {
        switch (size)
        {
            case Asteroid.AsteroidSize.Small: // spawns 2 small asteroids
                for (int i = childSpawnAmount; i > (int)size; i--) // (int)size will be 0
                {
                    if (asteroidSmall == null)
                    {
                        Debug.LogWarning("Small Asteroid prefab not assigned!");
                        return;
                    }
                    GameObject spawnedAsteroidSmall = Instantiate(asteroidSmall, position, transform.rotation);
                    spawnedAsteroidSmall.GetComponent<Asteroid>().InitializeSpawner(this);
                }
                break;
            case Asteroid.AsteroidSize.Medium: // spawns 2 medium asteroids
                for (int i = childSpawnAmount; i >= (int)size; i--) // (int)size will be 1
                {
                    if (asteroidMedium == null)
                    {
                        Debug.LogWarning("Medium Asteroid prefab not assigned!");
                        return;
                    }
                    GameObject spawnedAsteroidMedium = Instantiate(asteroidMedium, position, transform.rotation);
                    spawnedAsteroidMedium.GetComponent<Asteroid>().InitializeSpawner(this);
                }
                break;
            case Asteroid.AsteroidSize.Large: // spawns 1 large asteroid
                if (asteroidLarge == null)
                {
                    Debug.LogWarning("Large Asteroid prefab not assigned!");
                    return;
                }
                GameObject spawnedAsteroidLarge = Instantiate(asteroidLarge, position, transform.rotation);
                spawnedAsteroidLarge.GetComponent<Asteroid>().InitializeSpawner(this);
                break;
        }
    }
}