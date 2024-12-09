using UnityEngine;

public class BouncePadInteraction : MonoBehaviour
{
    public float bounceForce = 100000000f; // Force applied when hitting the bounce pad

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // Check if the collided object is on the BouncePad layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("BouncePad"))
        {
            Bounce();
        }
    }

    private void Bounce()
    {
        // Completely overwrite vertical velocity to ensure a strong upward motion
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0; // Reset vertical component
        rb.linearVelocity = velocity;

        // Apply bounce force directly as a velocity change (not impulse)
        rb.linearVelocity += Vector3.up * (bounceForce / rb.mass);
    }
}
