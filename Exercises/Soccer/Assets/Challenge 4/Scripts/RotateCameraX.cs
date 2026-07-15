using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200;
    public GameObject player;
    private InputAction controls;

    void Awake()
    {
        controls = new InputAction();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = controls.ReadValue<Vector2>().x;
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime);

        transform.position = player.transform.position; // Move focal point with player

    }
}
