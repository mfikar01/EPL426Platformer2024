using UnityEngine;
using System.Collections;


public class VictoryMusicTrigger : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusic; // Reference to the background music AudioSource
    public AudioSource victoryMusic;    // Reference to the victory music AudioSource

    [Header("Player Reference")]
    public GameObject player;           // Reference to the player GameObject

    public PauseMenuController pauseMenuController;

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;   // Time in seconds to fade out the background music

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Wait());
        // Check if the object colliding is the player
        if (other.gameObject == player)
        {
            // Start fading out the background music
            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                StartCoroutine(FadeOutBackgroundMusic());
            }
            else
            {
                Debug.LogWarning("Background music is either null or not playing.");
            }

            // Play the victory music after a short delay for a smoother transition
            if (victoryMusic != null && !victoryMusic.isPlaying)
            {
                victoryMusic.PlayDelayed(fadeDuration/2);
            }
            else
            {
                Debug.LogWarning("Victory music is either null or already playing.");
            }
            
            // Destroy(gameObject);

}
    }

    private IEnumerator FadeOutBackgroundMusic()
    {
        float startVolume = backgroundMusic.volume;

        while (backgroundMusic.volume > 0)
        {
            backgroundMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Ensure the volume is fully set to 0 and stop the background music
        backgroundMusic.volume = 0;
        backgroundMusic.Stop();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        if (pauseMenuController != null)
        {
            pauseMenuController.EndGame();
        }
    }
}
