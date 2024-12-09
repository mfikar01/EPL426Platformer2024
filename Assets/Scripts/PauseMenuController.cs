using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuCanvas; // Assign in Inspector (PauseMenuCanvas)

    [Header("Audio Clips")]
    public AudioSource pauseSound; // Assign the pause sound in Inspector
    public AudioSource resumeSound; // Assign the resume sound in Inspector

    private bool isPaused = false; // Pause state

    void Update()
    {
        // Check for ESC key to toggle pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        // Activate the pause menu
        pauseMenuCanvas.SetActive(true);

        // Play pause sound
        if (pauseSound != null)
        {
            pauseSound.Play();
        }

        // Pause game time
        Time.timeScale = 0f; // Stop time
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Deactivate the pause menu
        pauseMenuCanvas.SetActive(false);

        // Play resume sound
        if (resumeSound != null)
        {
           resumeSound.Play();
        }

        // Resume game time
        Time.timeScale = 1f; // Resume time
        isPaused = false;
    }

    public void QuitGame()
    {
        // Optionally handle quitting the game
        Debug.Log("Quit game clicked!");
        // Application.Quit(); // Uncomment this in the build
    }
}
