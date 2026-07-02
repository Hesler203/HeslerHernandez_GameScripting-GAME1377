using UnityEngine;

public class MoveLeftX : MonoBehaviour
{
    private const float LEFT_BOUND = -10f;
    [SerializeField] private float speed;

    void Update()
    {
        HandleMovementInBounds();
    }

    /// <summary>
    /// Moves the gameObject left at constant speed while game is active,
    /// destroys the object when past left bounds
    /// </summary>
    private void HandleMovementInBounds()
    {
        // While game is active, move to the left
        if (!GameManager.IsGameOver && transform.position.x > LEFT_BOUND)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World); // constant speed
        }
        else
        {
            Destroy(gameObject); // destroy when out of bounds
        }
    }
}
