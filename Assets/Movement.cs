using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    private Collision coll;
    private Animator animator; // Reference to the Animator
    [HideInInspector]
    public Rigidbody rb;

    [Space]
    [Header("Stats")]
    public float speed = 30;
    public float jumpForce = 1;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 100;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]
    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    void Start()
    {
        canMove = true;
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component

        // Lock Z-axis to keep movement constrained to 2D plane
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0);

        // Check for side mismatch and trigger turn-around
        if (x != 0 && Mathf.Sign(x) != side)
        {
            
            //animator.SetFloat("Turn", side);

            // Flip the side
            side = -side;

            // Rotate the character 180 degrees on the Y-axis

            transform.DORotate(new Vector3(0f, transform.eulerAngles.y + 180f, 0f), 0.2f, RotateMode.Fast)
                         .OnKill(() => CorrectRotationBasedOnSide());
            //animator.SetFloat("Turn", 0);
        }

        Walk(dir);

        // Update Animator parameters for movement
        bool isMoving = Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0;
        animator.SetBool("Moving", isMoving);
        animator.SetFloat("InputX", x);
        animator.SetFloat("InputY", y);

        // Wall grabbing and wall sliding conditions
        if (coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.useGravity = false;
            if (x > .2f || x < -.2f)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);

            float speedModifier = y > 0 ? .5f : 1;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, y * (speed * speedModifier), 0);
        }
        else
        {
            rb.useGravity = true;
        }

        // Wall sliding logic
        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (coll.onGround)
            {
                Jump(Vector3.up, false);
                animator.SetBool("Grounded", false); // Trigger jump animation
            }

            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        // Dashing
        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        // Ground touch detection
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;

            // Trigger grounded animation
            animator.SetBool("Grounded", true);
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (wallGrab || wallSlide || !canMove)
            return;
    }

    private void CorrectRotationBasedOnSide()
    {
        if (side == 1)
        {
            // Face right (0 degrees)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (side == -1)
        {
            // Face left (180 degrees)
            transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
        animator.SetBool("Jump", false);
        animator.SetBool("Victory", false);
    }

    private void Dash(float x, float y)
    {
        hasDashed = true;
        rb.linearVelocity = Vector3.zero;
        Vector3 dir = new Vector3(x, y, 0);

        rb.linearVelocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());

        // Trigger dash animation
        animator.SetBool("Victory", true); // Example: Use "Victory" for dashing
    }

    private void WallSlide()
    {
        animator.SetBool("Jump", false);
        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.linearVelocity.x > 0 && coll.onRightWall) || (rb.linearVelocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.linearVelocity.x;

        rb.linearVelocity = new Vector3(push, -slideSpeed, 0);
    }

    private void Walk(Vector3 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.linearVelocity = new Vector3(dir.x * speed, rb.linearVelocity.y, 0);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(dir.x * speed, rb.linearVelocity.y, 0), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector3 dir, bool wall)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
        rb.linearVelocity += dir * jumpForce;

        // Trigger jump animation
        animator.SetBool("Jump", true);
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        rb.useGravity = false;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.useGravity = true;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector3 wallDir = coll.onRightWall ? Vector3.left : Vector3.right;

        Jump((Vector3.up / 1.5f + wallDir / 1.5f), true);
        wallJumped = true;

        // Trigger wall jump animation
        animator.SetBool("Jump", true);
    }

    void RigidbodyDrag(float x)
    {
        rb.linearDamping = x;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
