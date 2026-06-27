using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    private GameManager GM;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float rotationSpeed = 250f;
    [SerializeField] private float thrustForce = 5f;

    private float rotationInput;
    private float thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GM = FindAnyObjectByType<GameManager>();
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

    private void HandleRotation()
    {
        if (rotationInput > .01f || rotationInput < .01f) // ensures deadzone
        {   // rotation will be clockwise with d-key
            transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleThrust()
    {
        if (thrustInput > .01f) // prevents backward movement plus ensures deadzone
        {
            rb.AddForce(transform.up.normalized * thrustInput * thrustForce);
        }
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Jump")) // set to spacebar
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Fire2")) // set to space-key
        {
            TeleportToRandomLocation();
        }
    }

    private void TeleportToRandomLocation()
    {
        float randomXInBounds = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomYInBounds = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);

        Vector3 teleportPosition = new Vector3(randomXInBounds, randomYInBounds);
        transform.position = teleportPosition;
    }

    void OnDestroy()
    {
        GM.GameOver();
    }
}
