using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource buttonClickSound;

    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For exiting in the editor
#endif
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
            buttonClickSound.Play();
    }
}