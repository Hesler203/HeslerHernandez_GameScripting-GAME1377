using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    public InputAction fireAction;
    private bool fired = false;
    private float timer;
    private float fireCooldown = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        fireAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fired)
        {
            if (fireAction.triggered) // On spacebar press, send dog
            {
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
                fired = true;
                timer = fireCooldown;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                fired = !fired;
            }
        }
    }
}
