using UnityEngine;

public class WarpLocation : MonoBehaviour
{
    [Header("Randomization Settings")]
    [SerializeField] private float randomizationInterval = 1f;
    [SerializeField] private float speed = 2f;
    private Vector3 awayDirection = Vector3.zero;

    void Start()
    {
        InvokeRepeating(nameof(SetNewWarpLocation), randomizationInterval, randomizationInterval);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            awayDirection = (transform.position - collision.gameObject.transform.position).normalized;
            transform.Translate(awayDirection * speed * Time.deltaTime);
        }
    }

    private void SetNewWarpLocation()
    {
        float randomXInBounds = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomYInBounds = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);
        transform.position = new Vector3(randomXInBounds, randomYInBounds);
    }
}
