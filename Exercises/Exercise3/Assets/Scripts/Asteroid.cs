using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Asteroid : MonoBehaviour
{
    private AsteroidSpawner spawner;

    public enum AsteroidSize { Small, Medium, Large }

    [Header("Settings")]
    [SerializeField] public AsteroidSize Size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;
    private Rigidbody2D rb;

    [Header("Animation References")]
    [SerializeField] private AnimationClip explodeAnimation;
    private AudioManager audioManager;
    private Animator animator;

    /// <summary>
    /// Sets the reference to the asteroid spawner object, to be called right after a new asteroid instance.
    /// </summary>
    /// <param name="asteroidSpawner">Reference to the asteroid spawner.</param>
    public void InitializeSpawnerRef(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    void Start()
    {
        audioManager = AudioManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        float randomX = Random.Range(-Random.value, Random.value);
        float randomY = Random.Range(-Random.value, Random.value);
        Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

        rb.AddForce(randomDirection * speed, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    /// <summary>
    /// Passes the next size down for children asteroids to be spawned before self-destruct,
    /// called on collision with bullet.
    /// </summary>
    public void BreakAsteroid()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("explode");
        audioManager.SFX.PlayOneShot(audioManager.SFXClips[1]);

        if (Size > 0)
        {
            SpawnChildren(Size - 1);
        }

        Destroy(gameObject, explodeAnimation.length);
    }

    /// <summary>
    /// Passes in the parent asteroid's position & the next asteroid size down to be spawned
    /// the spawn manager's SpawnAsteroid() method.
    /// </summary>
    /// <param name="childSize">The next smallest asteroid size to be spawned.</param>
    private void SpawnChildren(AsteroidSize childSize)
    {
        for (int i = 0; i < spawner.ChildSpawnAmount; i++)
        {
            spawner.SpawnAsteroid(transform.position, childSize);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<SpaceshipController>().SubtractLife();
        }
    }
}
