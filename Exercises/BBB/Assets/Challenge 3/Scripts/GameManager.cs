using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Global
/// </summary>
public class GameManager : MonoBehaviour
{
    // The static global variable all scripts can only read for current GameOver-state status
    public static bool IsGameOver { private set; get; }

    void Awake()
    {
        // state resets on new game/scene start
        IsGameOver = false;
    }

    /// <summary>
    /// Static method any script can access to set the GameOver state
    /// </summary>
    public static void GameOver()
    {
        IsGameOver = true;
        Debug.Log($" Game Over \n Your score was: {ScoreUpdater.Score} ");
    }
}