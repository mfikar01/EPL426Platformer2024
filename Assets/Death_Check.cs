using UnityEngine;

public class CharacterRespawnWithShader : MonoBehaviour
{
    public float minHeight = -10f; // The height threshold for triggering death
    public Vector3 respawnPosition; // The position to respawn the character
    public float respawnDelay = 2f; // Delay before respawn occurs

    private Rigidbody rb; // 3D Rigidbody component
    private bool isDead = false; // To prevent multiple triggers
    private MonoBehaviour[] movementScripts; // Scripts controlling movement
    private Animator animator; // Reference to Animator component

    public Material freezeMaterial; // Shader material to apply during freeze
    private Material[][] originalMaterials; // Store the original materials for all renderers
    private Renderer[] renderers; // All renderers in the character

    void Start()
    {
        // Set the starting position as the respawn point (can be customized)
        respawnPosition = transform.position;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get all movement-related scripts (or specific ones if known)
        movementScripts = GetComponents<MonoBehaviour>();

        // Get the Animator (if available)
        animator = GetComponent<Animator>();

        // Get all renderers in the character (including children)
        renderers = GetComponentsInChildren<Renderer>();

        // Store original materials for all renderers
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
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

        // Disable gravity and freeze the object
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity
            rb.linearVelocity = Vector3.zero; // Stop movement
            rb.constraints = RigidbodyConstraints.FreezeAll; // Freeze position and rotation
        }

        // Disable movement-related scripts
        foreach (var script in movementScripts)
        {
            script.enabled = false;
        }

        // Disable Animator to stop animations
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Apply freeze material to all renderers
        if (freezeMaterial != null)
        {
            foreach (var renderer in renderers)
            {
                var freezeMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < freezeMaterials.Length; i++)
                {
                    freezeMaterials[i] = freezeMaterial;
                }
                renderer.materials = freezeMaterials;
            }
        }

        // Call Respawn after a delay
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        // Reset character position
        transform.position = respawnPosition;

        // Reset Rigidbody settings
        if (rb != null)
        {
            rb.useGravity = true; // Restore gravity
            rb.constraints = RigidbodyConstraints.None; // Unfreeze
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Freeze rotation only
        }

        // Re-enable movement-related scripts
        foreach (var script in movementScripts)
        {
            script.enabled = true;
        }

        // Re-enable Animator if it was disabled
        if (animator != null)
        {
            animator.enabled = true;
        }

        // Restore original materials for all renderers
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        // Reset any necessary variables
        isDead = false;
    }
}
