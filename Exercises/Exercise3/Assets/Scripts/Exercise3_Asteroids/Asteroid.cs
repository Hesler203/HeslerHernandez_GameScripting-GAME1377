using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private Vector2 velocity;

    private AsteroidSpawner spawner;

    /// <summary>
    /// Sets the reference to the asteroid spawner object, to be called right after a new asteroid instance
    /// </summary>
    /// <param name="asteroidSpawner">reference to the asteroid spawner object</param>
    public void InitializeSpawner(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        float randomX = Random.Range(-Random.value, Random.value);
        float randomY = Random.Range(-Random.value, Random.value);
        velocity = new Vector2(randomX, randomY).normalized; // save only the random direction into velocity

        rb.AddForce(speed * velocity, ForceMode2D.Impulse); // instant speed in the random direction
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed); // rotates at random speed in range
    }

    /// <summary>
    /// Determines the next smallest size asteroids to be spawned and passes this size
    /// to SpawnChildren() before self-destruct
    /// </summary>
    private void BreakAsteroid()
    {
        int sizeDown = (int)size - 1;
        SpawnChildren((AsteroidSize)sizeDown);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Passes in the parent asteroid's position & the recieved asteroid size from argument to SpawnAsteroid()
    /// </summary>
    /// <param name="childSize"> the next smallest asteroid size to be spawned as children</param>
    private void SpawnChildren(AsteroidSize childSize)
    {
        spawner.SpawnAsteroid(transform.position, childSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.collider.gameObject);
        }
    }
}
