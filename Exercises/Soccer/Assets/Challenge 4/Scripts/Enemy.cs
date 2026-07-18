using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed;
    private Vector3 playerGoalDirection;
    public Vector3 PlayerDirection;

    private Rigidbody enemyRb;
    private SpawnManager spawnManager;

    /// <summary>
    /// Sets the reference to the spawnManager script, called during spawning.
    /// </summary>
    /// <param name="spawnManager">Reference to the spawnManager script.</param>
    public void SetSpawnerRef(SpawnManager spawnManager)
    {
        this.spawnManager = spawnManager;
    }

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerGoalDirection = GetDirectionToTarget(spawnManager.PlayerGoal);
        PlayerDirection = GetDirectionToTarget(spawnManager.Player.transform);
    }

    void FixedUpdate()
    {
        MoveToPlayerGoal();
    }

    /// <summary>
    /// Sets the direction vector from the enemy to the target, normalized & with zero vertical value.
    /// </summary>
    private Vector3 GetDirectionToTarget(Transform target)
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;
        return new Vector3(targetDirection.x, 0, targetDirection.z);
    }

    /// <summary>
    /// Applies a constant acceleration toward the player goal while
    /// linear drag caps & smoothens the movement speed.
    /// </summary>
    private void MoveToPlayerGoal()
    {
        enemyRb.AddForce(playerGoalDirection * speed, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy Goal" || other.gameObject.name == "Player Goal")
        {
            spawnManager.DecreaseEnemyCount();
            Destroy(gameObject);
        }
    }
}
