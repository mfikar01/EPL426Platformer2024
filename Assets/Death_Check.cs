using UnityEngine;

public class CharacterRespawnWithShader : MonoBehaviour
{
    public float minHeight = -10f; // The height threshold for triggering death
    public Vector3 respawnPosition; // The position to respawn the character
    public float respawnDelay = 2f; // Delay before respawn occurs
    public DeathCounter deathCounter; // Reference to the DeathCounter script

    private Collision coll;
    private Rigidbody rb; // 3D Rigidbody component
    private bool isDead = false; // To prevent multiple triggers
    private MonoBehaviour[] movementScripts; // Scripts controlling movement
    private Animator animator; // Reference to Animator component

    public Material freezeMaterial; // Shader material to apply during freeze
    private Material[][] originalMaterials; // Store the original materials for all renderers
    private Renderer[] renderers; // All renderers in the character

    public ParticleSystem deathParticle; // Particle system to play on death
    public AudioSource DeathAudio;

    public ScreenFade screenFade; // Reference to the ScreenFade script

    void Start()
    {
        coll = GetComponent<Collision>();
        respawnPosition = transform.position; // Set the starting position as the respawn point
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        movementScripts = GetComponents<MonoBehaviour>(); // Get all movement-related scripts
        animator = GetComponent<Animator>(); // Get the Animator component
        renderers = GetComponentsInChildren<Renderer>(); // Get all renderers in the character

        // Store original materials for all renderers
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    void Update()
    {
        // Check if the player falls below the minHeight or touches danger
        if (!isDead && (transform.position.y < minHeight || coll.onDanger))
        {
            TriggerDeath();
        }
    }

    void TriggerDeath()
    {
        isDead = true;
        deathCounter.IncrementDeathCount();

        // Trigger screen fade
        if (screenFade != null)
        {
            screenFade.FadeIn(0.5f); // Fade in over 0.5 seconds
        }
        else
        {
            Debug.LogWarning("ScreenFade reference is missing!");
        }

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

        PlayDeathParticle();

        // Call Respawn after a delay
        Invoke(nameof(Respawn), respawnDelay);
    }

    void PlayDeathParticle()
    {
        if (deathParticle != null)
        {
            deathParticle.transform.position = transform.position;
            deathParticle.Play();
        }

        if (DeathAudio != null)
        {
            DeathAudio.Play();
        }
        else
        {
            Debug.LogError("Death AudioSource is not assigned!");
        }
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

        // Trigger screen fade out
        if (screenFade != null)
        {
            screenFade.FadeOut(0.5f); // Fade out over 0.5 seconds
        }

        // Reset any necessary variables
        isDead = false;
    }

    public void SetRespawnPosition(Vector3 pos)
    {
        respawnPosition = pos;
    }
}
