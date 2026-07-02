using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);

        Destroy(this.gameObject, bulletLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            collision.collider.gameObject.SendMessage("BreakAsteroid"); 
            Destroy(this.gameObject);
        }
    }
}
