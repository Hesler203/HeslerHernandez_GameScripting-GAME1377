using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private AsteroidSpawner spawner;
    private Vector2 velocity;

    public void Initialize(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        float randomX = Random.Range(-Random.value, Random.value);
        float randomY = Random.Range(-Random.value, Random.value);
        velocity = new Vector2(randomX, randomY).normalized;

        rb.AddForce(speed * velocity, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void BreakAsteroid()
    {
        int sizeDown = (int)size - 1;
        SpawnChildren((AsteroidSize)sizeDown);

        spawner.decreaseAsteroidCounter();
        Destroy(this.gameObject);
    }

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
