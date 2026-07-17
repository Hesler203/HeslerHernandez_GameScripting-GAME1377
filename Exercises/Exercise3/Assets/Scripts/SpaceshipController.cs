using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidsPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float thrustDeadZone = .01f;
    [SerializeField] private float rotationDeadZone = .01f;
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 250f;
    private float thrustInput;
    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    /// <summary>
    /// Rotates the player with constant rotation speed along the world-space z-axis using the rotationInput.
    /// </summary>
    private void HandleRotation()
    {
        if (rotationInput > thrustDeadZone || rotationInput < thrustDeadZone)
        {
            transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Applies a forward accelaration to the player in local space using the thrustInput & thrustForce multipliers.
    /// </summary>
    private void HandleThrust()
    {
        if (thrustInput > rotationDeadZone)
        {
            rb.AddForce(transform.up * thrustInput * thrustForce, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// Checks for Spacebar input before firing a bullet.
    /// </summary>
    private void HandleFire()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            FireBullet();
        }
    }

    /// <summary>
    /// Spawns a bullet from the player's firePoint location.
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Checks for shift-key input before warping the player to a random location within bounds.
    /// </summary>
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Warp"))
        {
            WarpToRandomLocation();
        }
    }

    /// <summary>
    /// Sets the player's position to a random location within screen bounds.
    /// </summary>
    private void WarpToRandomLocation()
    {
        float randomXInBounds = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomYInBounds = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);
        transform.position = new Vector3(randomXInBounds, randomYInBounds);
    }

    void OnDestroy()
    {
        Debug.Log("Game Over");
        Debug.Break();
    }
}
