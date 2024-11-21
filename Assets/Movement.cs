using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody rb;  // Changed Rigidbody2D to Rigidbody

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

    // Removed "Polish" section for particle systems and other visuals.

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody>();  // Changed Rigidbody2D to Rigidbody

        // Lock Z-axis to keep movement constrained to 2D plane
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Debug.Log("Can Move: " + canMove);
        // Use Vector3 for 3D direction; Z is always zero to stay on 2D plane
        Vector3 dir = new Vector3(x, y, 0);

        Walk(dir);

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
            rb.useGravity = false;  // Gravity off for wall grab
            if (x > .2f || x < -.2f)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, y * (speed * speedModifier), 0);
        }
        else
        {
            rb.useGravity = true;  // Gravity back on when not grabbing wall
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
                Jump(Vector3.up, false);  // Changed Vector2 to Vector3
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
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        // Commented out visual particle effects for wall interactions
        // WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
        // Commented out particle effects for ground touch
        // jumpParticle.Play();
    }

    private void Dash(float x, float y)
    {
        // Commented out camera shake and visual effects for dashing
        // Camera.main.transform.DOComplete();
        // Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        // FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        rb.linearVelocity = Vector3.zero;  // Changed Vector2 to Vector3
        Vector3 dir = new Vector3(x, y, 0);

        rb.linearVelocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    private void WallSlide()
    {
        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.linearVelocity.x > 0 && coll.onRightWall) || (rb.linearVelocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.linearVelocity.x;

        rb.linearVelocity = new Vector3(push, -slideSpeed, 0);  // Changed to Vector3
    }

    private void Walk(Vector3 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.linearVelocity = new Vector3(dir.x * speed, rb.linearVelocity.y, 0);  // Keep Z at 0 for 2D movement
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, (new Vector3(dir.x * speed, rb.linearVelocity.y, 0)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector3 dir, bool wall)
    {
        // slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        // ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);  // Reset Y and Z
        rb.linearVelocity += dir * jumpForce;

        // particle.Play();
    }

    IEnumerator DashWait()
    {
        // Commented out particle effects for dashing
        // FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        // dashParticle.Play();
        rb.useGravity = false;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        // dashParticle.Stop();
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
    }

    void RigidbodyDrag(float x)
    {
        rb.linearDamping = x;
    }

    // Commented out particle effects for wall interactions
    // void WallParticle(float vertical)
    // {
    //     var main = slideParticle.main;
    //
    //     if (wallSlide || (wallGrab && vertical < 0))
    //     {
    //         slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
    //         main.startColor = Color.white;
    //     }
    //     else
    //     {
    //         main.startColor = Color.clear;
    //     }
    // }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
