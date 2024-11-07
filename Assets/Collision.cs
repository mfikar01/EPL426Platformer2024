using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]
    public float collisionRadius = 0.25f;
    public Vector3 bottomOffset = new Vector3(0, -0.55f, 0);
    public Vector3 rightOffset = new Vector3(0.55f, 0, 0);
    public Vector3 leftOffset = new Vector3(-0.55f, 0, 0);

    void Update()
    {
        // Check if the player is on the ground (using a 3D OverlapSphere)
        onGround = Physics.CheckSphere(transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics.CheckSphere(transform.position + rightOffset, collisionRadius, groundLayer)
                || Physics.CheckSphere(transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics.CheckSphere(transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics.CheckSphere(transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;

   

        if (onWall)
        {
            Debug.Log("Collision detected with a wall.");
        }

        if (onRightWall)
        {
            Debug.Log("Collision detected with the right wall.");
        }

        if (onLeftWall)
        {
            Debug.Log("Collision detected with the left wall.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Use Gizmos.DrawWireSphere for 3D collision visualization
        Gizmos.DrawWireSphere(transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere(transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere(transform.position + leftOffset, collisionRadius);
    }
}
