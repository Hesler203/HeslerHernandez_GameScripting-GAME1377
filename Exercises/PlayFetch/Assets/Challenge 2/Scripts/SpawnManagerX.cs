using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] ballPrefabs;

    private float spawnLimitXLeft = -22.0f;
    private float spawnLimitXRight = 7.0f;
    private float spawnPosY = 30.0f;

    private float startDelay = 1.0f;
    private float spawnInterval = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomBall", startDelay, spawnInterval);
    }

    // Spawn random ball at random x position at top of play area
    void SpawnRandomBall()
    {
        // Generate random ball index and random spawn position
        Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, 0);
        // Reference a random ball within the ballPrefabs[]
        GameObject ball = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
        // instantiate a ball at random spawn location
        Instantiate(ball, spawnPos, ball.transform.rotation);
    }
}
