using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size.
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    private float playerSafeDistance = 3;

    [SerializeField] private GameObject asteroidSmall;
    [SerializeField] private GameObject asteroidMedium;
    [SerializeField] private GameObject asteroidLarge;

    [SerializeField] private int initalSpawnAmount; // set to 5 in the inspector
    [SerializeField] private int childSpawnAmount = 2;

    private float randomXInBounds;
    private float randomYInBounds;
    private Vector3 randomSpawnLocation;

    [SerializeField] private float minSpawnDelay = 3f;
    [SerializeField] private float maxSpawnDelay = 6f;
    private float randomSpawnTimer;

    private float asteroidCounter = 0;

    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;

        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;

        SpawnInitialAsteroids();

        setRandomSpawnTimer();
        Invoke(nameof(SpawnRecurringAsteroids), randomSpawnTimer);
    }

    private void SpawnInitialAsteroids()
    {
        // Spawn initial asteroids at random positions. Ensure that they do not spawn where the player is located.
        for (int i = 0; i < initalSpawnAmount; i++)
        {
            setRandomSpawnLocation();
            if (IsSpawnDistanceSafe())
            {
                SpawnAsteroid(randomSpawnLocation, Asteroid.AsteroidSize.Large);
            }
        }
    }

    private bool IsSpawnDistanceSafe()
    {
        float spawnDistance = Vector3.Distance(Vector3.zero, randomSpawnLocation);

        while (spawnDistance < playerSafeDistance)
        {
            setRandomSpawnLocation();
            spawnDistance = Vector3.Distance(Vector3.zero, randomSpawnLocation);
        }
        return true;
    }

    private void SpawnRecurringAsteroids()
    {
        if (asteroidCounter == 0)
        {
            SpawnAsteroid(randomSpawnLocation, Asteroid.AsteroidSize.Large);
        }

        setRandomSpawnTimer();
        Invoke(nameof(SpawnRecurringAsteroids), randomSpawnTimer);
    }

    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size)
    {
        // Spawn an asteroid at the location specified by position parameter with the size specified by the size parameter.
        switch (size)
        {
            case Asteroid.AsteroidSize.Small:
                for (int i = childSpawnAmount; i > (int)size; i--)
                {
                    GameObject spawnedAsteroidSmall = Instantiate(asteroidSmall, position, transform.rotation);

                    spawnedAsteroidSmall.GetComponent<Asteroid>().Initialize(this);
                    asteroidCounter++;
                }
                break;
            case Asteroid.AsteroidSize.Medium:
                for (int i = childSpawnAmount; i >= (int)size; i--)
                {
                    GameObject spawnedAsteroidMedium = Instantiate(asteroidMedium, position, transform.rotation);

                    spawnedAsteroidMedium.GetComponent<Asteroid>().Initialize(this);
                    asteroidCounter++;
                }
                break;
            case Asteroid.AsteroidSize.Large:
                GameObject spawnedAsteroidLarge = Instantiate(asteroidLarge, position, transform.rotation);

                spawnedAsteroidLarge.GetComponent<Asteroid>().Initialize(this);
                asteroidCounter++;
                break;
        }
    }

    private void setRandomSpawnLocation()
    {
        randomXInBounds = Random.Range(spawnXMin, spawnXMax);
        randomYInBounds = Random.Range(spawnYMin, spawnYMax);

        randomSpawnLocation = new Vector3(randomXInBounds, randomYInBounds);
    }

    private void setRandomSpawnTimer()
    {
        randomSpawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    public void decreaseAsteroidCounter()
    {
        asteroidCounter--;
    }
}