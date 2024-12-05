using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player character
    public Vector2 offset;   // Distance from the center of the camera
    public Vector2 cameraBounds; // Edges of the screen before camera starts moving
    public float smoothSpeed = 0.125f; // Smoothing for the camera movement

    private Vector3 targetPosition;

    void FixedUpdate()
    {
        if (target == null) return;

        // Camera movement logic here (similar to LateUpdate logic)
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        float deltaX = target.position.x - transform.position.x;
        float deltaY = target.position.y - transform.position.y;

        if (Mathf.Abs(deltaX) > cameraBounds.x)
            targetPosition.x = target.position.x - Mathf.Sign(deltaX) * cameraBounds.x;
        else
            targetPosition.x = transform.position.x;

        if (Mathf.Abs(deltaY) > cameraBounds.y)
            targetPosition.y = target.position.y - Mathf.Sign(deltaY) * cameraBounds.y;
        else
            targetPosition.y = transform.position.y;

        targetPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}