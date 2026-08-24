using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);

        Destroy(gameObject, bulletLifetime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Asteroid"))
        {
            collider.GetComponent<Asteroid>().BreakAsteroid();

            Destroy(gameObject);
        }
    }
}
