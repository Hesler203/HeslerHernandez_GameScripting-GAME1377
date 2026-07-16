using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform focalPoint;
    [SerializeField] private InputActionAsset inputActions;
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
    }
}
