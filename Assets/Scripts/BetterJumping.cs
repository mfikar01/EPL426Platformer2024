using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody rb;
    public float fallMultiplier = 5.0f;
    public float lowJumpMultiplier = 20f;

    public float maxJumpTime = 0.25f; // Maximum time to hold the jump button for full jump
    private float jumpTimeCounter;   // Tracks how long the jump button is held
    private bool isJumping;          // Whether the player is in the middle of a jump

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is holding the jump button
        if (Input.GetButton("Jump") && isJumping)
        {
            jumpTimeCounter += Time.deltaTime;

            // Stop applying upward force after maxJumpTime is exceeded
            if (jumpTimeCounter > maxJumpTime)
            {
                isJumping = false;
            }
        }
        else
        {
            isJumping = false; // Stop jumping if the button is released
        }

        // Custom gravity adjustments
        if (rb.linearVelocity.y < 0)
        {
            // Falling: apply fallMultiplier for faster descent
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            // Short hop: stop upward force when button is released
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Reset jump variables when grounded
        if (rb.linearVelocity.y <= 0)
        {
            jumpTimeCounter = 0;
            isJumping = true;
        }
    }
}
