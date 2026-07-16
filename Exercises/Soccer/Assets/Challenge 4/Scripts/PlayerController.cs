using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalStrength = 10;

    [Header("Movement")]
    [SerializeField] private Transform focalPoint;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Vector3 startingPosition = new Vector3(0, 1, -7);
    [SerializeField] private float moveDeadzone = 0.001f;
    [SerializeField] private float speed = 3;
    private Rigidbody playerRb;
    private InputAction moveAction;
    private float depthInput;
    private Vector3 moveDirection;

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
        moveDirection = focalPoint.forward;
        depthInput = moveAction.ReadValue<Vector2>().y;
    }

    void FixedUpdate()
    {
        if (depthInput > moveDeadzone || depthInput < -moveDeadzone)
        {
            playerRb.AddForce(moveDirection * depthInput * speed, ForceMode.Acceleration);
        }


    public void ResetPlayer()
    {
        transform.position = startingPosition;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        focalPoint.GetComponent<RotateCamera>().ResetCamera();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayDirection = -other.gameObject.GetComponent<Enemy>().playerDirection;
                enemyRb.AddForce(awayDirection * normalStrength, ForceMode.Impulse);
        }
    }
}
