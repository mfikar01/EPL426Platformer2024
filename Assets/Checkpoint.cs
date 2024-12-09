using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject player; // Reference to the player GameObject

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.gameObject == player)
        {
            // Update the player's respawn position
            CharacterRespawnWithShader playerRespawn = player.GetComponent<CharacterRespawnWithShader>();
            if (playerRespawn != null)
            {
                playerRespawn.SetRespawnPosition(transform.position);
                Debug.Log("Checkpoint updated to: " + transform.position);
            }
            else
            {
                Debug.LogWarning("PlayerRespawn script is missing on the player object.");
            }

            // Optional: Play a sound or visual effect to confirm the checkpoint
        }
    }
}
