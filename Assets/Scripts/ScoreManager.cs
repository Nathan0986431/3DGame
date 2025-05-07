using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance for global access
    public TMP_Text scoreText;           // Reference to the TMP text to display the score

    private int score = 0;

    void Awake()
    {
        // Set up the Singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // Initialize the score display
        UpdateScoreText();
    }

    // Method to add points to the score
    public void AddPoint()
    {
        score++;
        UpdateScoreText();
    }

    // Method to update the score text in the UI
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Public property to access the current score
    public int CurrentScore
    {
        get { return score; }
    }

    // Optionally, you can add a reset method to reset score at the start of the game or when needed
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}