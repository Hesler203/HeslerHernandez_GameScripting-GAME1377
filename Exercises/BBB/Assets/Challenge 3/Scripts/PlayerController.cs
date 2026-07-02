using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction floatAction;
    private const float TOP_BOUND = 15f;

    private Rigidbody playerRb;
    [SerializeField] private float floatForce;
    [SerializeField] private float initialForce = 5f;
    [SerializeField] private float bounceForce = 15f;
    [SerializeField] private float gravityModifier = 1.5f;

    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem fireworksParticle;
    private AudioManager audioManager;

    void Start()
    {
        floatAction.Enable();
        audioManager = FindAnyObjectByType<AudioManager>();

        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * initialForce, ForceMode.Impulse);

    }

    void FixedUpdate()
    {
        HandleFloat();
    }

    /// <summary>
    /// Adds an upward acceleration to the player while they are pressing the spacebar & game is active,
    /// Player will reach a terminal velocity determined by the Rigidbody's linear drag,
    /// The player's max height is clamped to a set Top Bound
    /// </summary>
    private void HandleFloat()
    {
        // Clamp the player's max height
        if (transform.position.y < TOP_BOUND)
        {
            // While space is pressed and the game is active, float up
            if (floatAction.IsPressed() && !GameManager.IsGameOver)
            {
                playerRb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, TOP_BOUND);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and trigger GameOver
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            audioManager.PlaySoundEffect(other.gameObject.tag);
            GameManager.GameOver();
            Destroy(other.gameObject);
        }

        // if player collides with ground, bounce
        if (other.gameObject.CompareTag("Ground"))
        {
            audioManager.PlaySoundEffect("Bounce");
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if player collides with money, fireworks
        if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            audioManager.PlaySoundEffect(other.gameObject.tag);
            Destroy(other.gameObject);
        }
    }
}
