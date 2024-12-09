using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask Danger;

    [Space]
    public bool onDanger;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]
    public Vector3 groundBoxSize = new Vector3(0.5f, 0.1f, 0.5f); // Size of the ground detection box
    public Vector3 wallBoxSize = new Vector3(0.1f, 1.0f, 0.5f);   // Size of the wall detection box

    // Adjust offsets for higher boxes
    public Vector3 upOffset = new Vector3(0, 0.45f, 0); // Slightly higher than before
    public Vector3 bottomOffset = new Vector3(0, -0.45f, 0); // Slightly higher than before
    public Vector3 rightOffset = new Vector3(0.1f, 0.2f, 0); // Higher to avoid ground overlap
    public Vector3 leftOffset = new Vector3(-0.1f, 0.2f, 0); // Higher to avoid ground overlap

    void Update()
    {
        bool upDanger = Physics.CheckBox(transform.position + upOffset, groundBoxSize / 2, Quaternion.identity, Danger);
        bool bottomDanger = Physics.CheckBox(transform.position + bottomOffset, groundBoxSize / 2, Quaternion.identity, Danger);
        bool rightDanger = Physics.CheckBox(transform.position + rightOffset, wallBoxSize / 2, Quaternion.identity, Danger);
        bool leftDanger = Physics.CheckBox(transform.position + leftOffset, wallBoxSize / 2, Quaternion.identity, Danger);

        onDanger = bottomDanger || rightDanger || leftDanger || upDanger;

        // Debug individual results
        if (bottomDanger) Debug.Log("Danger detected below!");
        if (rightDanger) Debug.Log("Danger detected to the right!");
        if (leftDanger) Debug.Log("Danger detected to the left!");
        // Check if the player is on the ground (using a box)
        onGround = Physics.CheckBox(transform.position + bottomOffset, groundBoxSize / 2, Quaternion.identity, groundLayer);

        // Check if the player is touching a wall (using boxes for left and right walls)
        onRightWall = Physics.CheckBox(transform.position + rightOffset, wallBoxSize / 2, Quaternion.identity, groundLayer);
        onLeftWall = Physics.CheckBox(transform.position + leftOffset, wallBoxSize / 2, Quaternion.identity, groundLayer);

        // Consolidate wall checks
        onWall = onRightWall || onLeftWall;

        // Determine the wall side
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

        // Draw boxes to visualize collision zones
        Gizmos.DrawWireCube(transform.position + upOffset, groundBoxSize);
        Gizmos.DrawWireCube(transform.position + bottomOffset, groundBoxSize);
        Gizmos.DrawWireCube(transform.position + rightOffset, wallBoxSize);
        Gizmos.DrawWireCube(transform.position + leftOffset, wallBoxSize);
    }
}