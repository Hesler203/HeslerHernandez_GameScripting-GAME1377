using UnityEngine;

public class DestroyOutOfBoundsX : MonoBehaviour
{
    private float leftLimit = -25.0f;
    private float bottomLimit = -5.0f;

    // Update is called once per frame
    void Update()
    {
        // Destroy dogs if x position less than left limit
        if (this.transform.position.x < leftLimit)
        {
            Destroy(gameObject);
        }
        // Destroy balls if y position is less than bottomLimit
        if (this.transform.position.y < bottomLimit)
        {
            Destroy(gameObject);
        }
    }
}
