using UnityEngine;

public class CharacterRespawn : MonoBehaviour
{
    public float minHeight = 10f; // The height threshold for triggering death
    public Vector3 respawnPosition; // The position to respawn the character
    public Animator animator; // Reference to the Animator component
    public float respawnDelay = 0.2f; // Delay before respawn occurs

    private bool isDead = false; // To prevent multiple triggers

    void Start()
    {
        // Set the starting position as the respawn point (can be customized)
        respawnPosition = transform.position;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Check if the player falls below the Min_Height
        if (!isDead && transform.position.y < minHeight)
        {
            TriggerDeath();
        }
    }

    void TriggerDeath()
    {
        isDead = true;

        // Play death animation if Animator exists
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Call Respawn after a delay
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        // Reset character position
        transform.position = respawnPosition;

        // Reset any necessary variables
        isDead = false;

        // Optional: Reset animations
        if (animator != null)
        {
            animator.SetTrigger("Respawn");
        }
    }
}
