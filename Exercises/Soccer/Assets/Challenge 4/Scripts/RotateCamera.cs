using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private InputActionAsset inputActions;

    [Header("Settings")]
    [SerializeField] private float lookSpeed = 10f;
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

    void Update()
    {
        float horizontalInput = lookAction.ReadValue<Vector2>().x;
        transform.Rotate(Vector3.up * horizontalInput * lookSpeed * Time.deltaTime);

        transform.position = player.transform.position;
    }
}
