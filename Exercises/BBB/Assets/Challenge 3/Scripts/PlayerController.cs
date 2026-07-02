using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction floatAction;

    private Rigidbody playerRb;
    [SerializeField] private float floatForce;
    [SerializeField] private float initialForce = 5f;
    [SerializeField] private float gravityModifier = 1.5f;

    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem fireworksParticle;

    void Start()
    {
        floatAction.Enable();

        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * initialForce, ForceMode.Impulse);

    }

    void FixedUpdate()
    {
        HandleFloat();
    }

    private void HandleFloat()
    {
            // While space is pressed and the game is active, float up
            if (floatAction.IsPressed() && !GameManager.IsGameOver)
            {
                playerRb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
            }
        }

    void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and trigger GameOver

    }

}
