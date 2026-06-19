using UnityEngine;

public class DetectCollisionsX : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Game Over");
        }
        else if (other.gameObject.CompareTag("Dog"))
        {
            Destroy(gameObject);
        }
    }
}
