using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Timer")]
    public float gameDuration = 60f;
    private float timer;
    private bool gameEnded = false;

    [Header("UI Elements")]
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI restartPromptText;

    public GameObject scoreUI; // Reference to the score UI (e.g., TMP_Text or other)
    public GameObject ammoUI;  // Reference to the ammo UI (e.g., TMP_Text or other)

    private ScoreManager scoreManager;

    void Start()
    {
        timer = 0f;
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (finalScoreText != null)
            finalScoreText.text = "";

        if (restartPromptText != null)
            restartPromptText.text = "";

        // Ensure the ScoreManager is present in the scene
        scoreManager = ScoreManager.Instance;
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager not found in the scene.");
        }

        // Make sure the UI is visible during gameplay
        if (scoreUI != null) scoreUI.SetActive(true);
        if (ammoUI != null) ammoUI.SetActive(true);
    }

    void Update()
    {
        if (gameEnded)
        {
            // Check if the mouse was clicked (0 means left-click)
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= gameDuration)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        Debug.Log("Time's Up :P");

        Time.timeScale = 0f;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "Time's Up :P";

        if (finalScoreText != null && scoreManager != null)
        {
            finalScoreText.text = $"Final Score: {scoreManager.CurrentScore}";
        }

        if (restartPromptText != null)
            restartPromptText.text = "Click anywhere to restart";

        // Hide the score and ammo UI when the game ends
        if (scoreUI != null) scoreUI.SetActive(false);
        if (ammoUI != null) ammoUI.SetActive(false);
    }
}
