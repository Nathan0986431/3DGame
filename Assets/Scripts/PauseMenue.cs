using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public bool IsPaused { get; private set; }

    [Header("UI")]
    public GameObject pauseMenuUI;

    [Header("Audio")]
    [SerializeField] private AudioSource buttonClickSound;
    [SerializeField] private AudioSource globalAudio; // Used for mute/unmute

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        IsPaused = false;
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame()
    {
        PlayClickSound();

        IsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        IsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnExitButtonClicked()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your actual main menu scene name
    }

    public void OnMuteButtonClicked()
    {
        PlayClickSound();

        if (globalAudio != null)
        {
            globalAudio.mute = !globalAudio.mute;
        }
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
}