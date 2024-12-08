using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Assign in Inspector (PauseMenuCanvas)
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

        // Make the game slightly darker (Overlay is part of PauseMenuCanvas)
        Time.timeScale = 0f; // Stop time
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Deactivate the pause menu
        pauseMenuCanvas.SetActive(false);

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
