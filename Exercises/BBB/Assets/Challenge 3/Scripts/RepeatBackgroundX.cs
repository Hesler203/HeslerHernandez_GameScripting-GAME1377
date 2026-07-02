using UnityEngine;

public class RepeatBackgroundX : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    [SerializeField] private float scrollSpeed = 5f;

    private void Start()
    {
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; // set to half the background width
        startPos = transform.position; // set the default starting position
    }

    private void Update()
    {
        HandleScroll();
    }

    /// <summary>
    /// Moves the background left at constant speed while game is active, resetting to its
    /// start position when halfway scrolled to give the illusion of an infinite background
    /// </summary>
    private void HandleScroll()
    {
        // If game is not over, continue background scroll
        if (!GameManager.IsGameOver)
        {
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime); // constant scrolling

            // If background moves left by its repeat width, move it back to start position
            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
    }
}


