using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour
{
    [Header("Settings")]
    public float respawnTime = 3f; // Time before the item reappears
    public GameObject player;      // Reference to the player GameObject

    private Renderer itemRenderer;
    private Collider itemCollider;

    void Start()
    {
        // Get references to the item's Renderer and Collider
        itemRenderer = GetComponent<Renderer>();
        itemCollider = GetComponent<Collider>();

        if (player == null)
        {
            Debug.LogError("Player reference is not set in PickupItem script!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is the one picking up the item
        if (other.gameObject == player)
        {
            // Access the Movement script and reset the hasDashed variable
            Movement movementScript = player.GetComponent<Movement>();
            if (movementScript != null)
            {
                movementScript.ResetDash(); // Call a method in Movement to handle resetting
                Debug.Log("Item picked up: hasDashed reset to false.");
            }
            else
            {
                Debug.LogError("Movement script not found on the player!");
            }

            // Disable the item and start respawn coroutine
            StartCoroutine(RespawnItem());
        }
    }

    private IEnumerator RespawnItem()
    {
        // Make the item invisible and inactive
        itemRenderer.enabled = false;
        itemCollider.enabled = false;

        // Wait for the respawn time
        yield return new WaitForSeconds(respawnTime);

        // Make the item visible and active again
        itemRenderer.enabled = true;
        itemCollider.enabled = true;
    }
}
