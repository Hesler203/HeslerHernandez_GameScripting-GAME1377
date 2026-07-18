using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private Vector3 powerUpOffset = new Vector3(0, -0.6f, 0);
    [SerializeField] private bool hasPowerup;
    [SerializeField] private int powerUpDuration = 5;
    [SerializeField] private float powerupStrength = 25;
    [SerializeField] private float normalStrength = 10;

    [Header("Movement")]
    [SerializeField] private Transform focalPoint;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Vector3 startingPosition = new Vector3(0, 1, -7);
    [SerializeField] private float moveDeadzone = 0.001f;
    [SerializeField] private float speed = 3;
    private Rigidbody playerRb;
    private InputAction moveAction;
    private InputAction boostAction;
    private float depthInput;
    private Vector3 moveDirection;
    private bool boostPressed = false;

    [Header("Smoke Effect")]
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private Vector3 smokeOffset = new Vector3(0, -0.6f, 0.5f);

    void Awake()
    {
        moveAction = inputActions.FindAction("Move", true);
        boostAction = inputActions.FindAction("Boost", true);
    }

    void OnEnable()
    {
        moveAction.Enable();
        boostAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        boostAction.Disable();
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        powerupIndicator.transform.position = transform.position + powerUpOffset;
        smokeParticles.transform.position = transform.position + smokeOffset;

        moveDirection = focalPoint.forward;
        depthInput = moveAction.ReadValue<float>();

        if (boostAction.IsPressed())
        {
            boostPressed = true;
            smokeParticles.Play();
        }
        else
        {
            boostPressed = false;
            smokeParticles.Stop();
        }
    }

    void FixedUpdate()
    {
        if (depthInput > moveDeadzone || depthInput < -moveDeadzone)
        {
            playerRb.AddForce(moveDirection * depthInput * speed, ForceMode.Acceleration);
        }

        if (boostPressed)
        {
            playerRb.AddForce(moveDirection * depthInput, ForceMode.VelocityChange);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            spawnManager.DecreasePowerupCount();

            ActivatePowerup();
            StartCoroutine(nameof(PowerupCooldown));
        }
    }

    /// <summary>
    /// Enables the powerup Indicator object & sets hasPowerup to true.
    /// </summary>
    private void ActivatePowerup()
    {
        powerupIndicator.SetActive(true);
        hasPowerup = true;
    }

    /// <summary>
    /// Starts a cooldown timer which after ending deactivates the powerup & stops the corroutine.
    /// </summary>
    /// <returns>A float time to yield the corroutine call</returns>
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        DeactivatePowerup();
        StopCoroutine(nameof(PowerupCooldown));
    }

    /// <summary>
    /// Disables the powerup Indicator object & sets hasPowerup to false.
    /// </summary>
    private void DeactivatePowerup()
    {
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    /// <summary>
    /// Hides all vfx & resets player related stats for a new wave of enemy spawns.
    /// </summary>
    public void ResetPlayer()
    {
        transform.position = startingPosition;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        focalPoint.GetComponent<RotateCamera>().ResetCamera();

        DeactivatePowerup();
        smokeParticles.Stop();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayDirection = -other.gameObject.GetComponent<Enemy>().playerDirection;

            if (hasPowerup)
            {
                enemyRb.AddForce(awayDirection * powerupStrength, ForceMode.Impulse);
            }
            else
            {
                enemyRb.AddForce(awayDirection * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
