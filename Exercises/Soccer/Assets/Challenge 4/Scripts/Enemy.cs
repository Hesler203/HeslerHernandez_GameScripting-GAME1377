using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed;

    private Rigidbody enemyRb;


    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveToPlayerGoal();
    }

    /// <summary>
    /// Applies a constant acceleration toward the player goal while
    /// linear drag caps & smoothens the movement speed.
    /// </summary>
    private void MoveToPlayerGoal()
    {
    }

    void OnCollisionEnter(Collision other)
    {
    }
}
