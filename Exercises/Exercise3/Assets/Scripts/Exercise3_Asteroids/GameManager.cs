using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        StartMessage();
    }

        void Update()
    {
        if (CheckForRestartInput())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private bool CheckForRestartInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            return true;
        }
        return false;
    }

    public void GameOver()
    {
        Debug.Log("Game Over. Press R-key to try again.");
        Time.timeScale = 0.4f;
    }

    private void StartMessage()
    {
        Debug.Log("Clear the Asteroids");
    }
}

