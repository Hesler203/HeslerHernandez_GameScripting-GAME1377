using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    // The static global variable all scripts can only read current Score count
    public static int Score { private set; get; }
    private static TextMeshProUGUI scoreLabel;

    void Awake()
    {
        // score resets on new game/scene start
        Score = 0;
        scoreLabel = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Static method any script can access to increase the score count
    /// </summary>
    public static void IncreaseScore()
    {
        Score++;
        scoreLabel.text = $"Score: {Score}";
    }
}
