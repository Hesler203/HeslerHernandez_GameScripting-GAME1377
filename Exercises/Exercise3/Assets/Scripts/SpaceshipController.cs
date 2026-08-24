using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Animator))]
public class SpaceshipController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform warpLocation;
    private Rigidbody2D rb;
    private PolygonCollider2D playerCollider;
    private Animator animator;
    private AudioManager audioManager;

    [Header("Settings")]
    [SerializeField] public int Lives = 3;
    [SerializeField] private bool didShoot = false;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float shootCooldown = .2f;
    [SerializeField] private float thrustDeadZone = .01f;
    [SerializeField] private float rotationDeadZone = .01f;
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 250f;
    private float thrustInput;
    private float rotationInput;
    private float timer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        audioManager = AudioManager.Instance;
        asteroidSpawner = FindAnyObjectByType<AsteroidSpawner>();
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
        if (thrustInput > rotationDeadZone && !animator.GetBool("didCrash"))
        {
            rb.AddForce(transform.up * thrustInput * thrustForce, ForceMode2D.Force);
            if (!animator.GetBool("isThrust"))
            {
                animator.SetBool("isThrust", true);
                audioManager.SFX.PlayOneShot(audioManager.SFXClips[2]);
            }
        }
        else
        {
            animator.SetBool("isThrust", false);
        }
    }

    /// <summary>
    /// Checks for Spacebar input before firing a bullet.
    /// </summary>
    private void HandleFire()
    {
        if (isInvincible)
        {
            return;
        }

        if (Input.GetButtonDown("Shoot") && !didShoot)
        {
            FireBullet();
            audioManager.SFX.PlayOneShot(audioManager.SFXClips[3]);
            didShoot = true;
            timer = shootCooldown;
        }

        if (didShoot)
        {
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                return;
            }
            timer = 0;
        }
        didShoot = false;
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
            audioManager.SFX.PlayOneShot(audioManager.SFXClips[4]);
            animator.SetBool("isWarp", true);
        }
    }

    private void WarpToSafeLocation()
    {
        audioManager.SFX.PlayOneShot(audioManager.SFXClips[4]);
        transform.position = warpLocation.position;
    }

    public void SubtractLife()
    {
        Lives--;

        if (Lives == 0)
        {
            Debug.Log("Game Over");
            Debug.Break();
        }
        else
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        animator.SetBool("didCrash", true);
        audioManager.SFX.PlayOneShot(audioManager.SFXClips[5]);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        isInvincible = true;
        playerCollider.enabled = false;
    }

    private void ResetPlayer()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void EndInvincibility()
    {
        isInvincible = false;
        playerCollider.enabled = true;
    }

    public void DisableAnimationBool(string boolName)
    {
        animator.SetBool(boolName, false);
    }

    public void EnableAnimationBool(string boolName)
    {
        animator.SetBool(boolName, true);
    }
}