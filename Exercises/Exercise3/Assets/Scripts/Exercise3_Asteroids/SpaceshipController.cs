using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float rotationSpeed = 250f;
    [SerializeField] private float thrustForce = 5f;
    private float rotationInput;
    private float thrustInput;
    [SerializeField] private float deadZone = .01f;


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
    /// Rotates the player with constant rotation speed along the world-space z-axis using the rotationInput
    /// </summary>
    private void HandleRotation()
    {
        if (rotationInput > deadZone || rotationInput < deadZone)
        {
            transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Applies a forward accelaration to the player in local space using the thrustInput & thrustForce multipliers
    /// </summary>
    private void HandleThrust()
    {
        if (thrustInput > deadZone)
        {
            rb.AddRelativeForce(Vector3.up * thrustInput * thrustForce, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// Checks for Spacebar input before firing a bullet
    /// </summary>
    private void HandleFire()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            FireBullet();
        }
    }

    /// <summary>
    /// Spawns a bullet from the player's firePoint location
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Checks for shift-key input before warping the player to a random location within bounds
    /// </summary>
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Warp"))
        {
            WarpToRandomLocation();
        }
    }

    /// <summary>
    /// Sets the player's position to a random location within screen bounds
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
