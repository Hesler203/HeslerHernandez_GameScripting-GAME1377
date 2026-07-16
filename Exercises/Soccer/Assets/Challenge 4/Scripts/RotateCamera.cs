using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private InputActionAsset inputActions;

    [Header("Settings")]
    [SerializeField] private float lookSpeed = 10f;
    private Quaternion startingRotation;
    private InputAction lookAction;

    void Awake()
    {
        lookAction = inputActions.FindAction("Look", true);
    }

    void OnEnable()
    {
        lookAction.Enable();
    }

    void OnDisable()
    {
        lookAction.Disable();
    }

    void Start()
    {
        startingRotation = transform.rotation;
    }

    void Update()
    {
        float horizontalInput = lookAction.ReadValue<Vector2>().x;
        transform.Rotate(Vector3.up * horizontalInput * lookSpeed * Time.deltaTime);

        transform.position = player.transform.position;
    }

    /// <summary>
    /// Sets the focal point's rotation to the inital rotation stored at Start().
    /// </summary>
    public void ResetCamera()
    {
        transform.rotation = startingRotation;
    }
}
