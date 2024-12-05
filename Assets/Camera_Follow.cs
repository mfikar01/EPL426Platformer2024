using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player character
    public Vector2 offset;   // Max distance the camera can shift from the target
    public Vector2 cameraBounds; // Edges of the screen before the camera starts moving
    public float smoothSpeed = 0.3f; // Higher values for slower, smoother movement
    public float minCameraHeight = 0f; // Minimum height the camera can move to

    private Vector3 velocity = Vector3.zero; // Used by SmoothDamp for smoothing

    void FixedUpdate()
    {
        if (target == null) return;

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // Get the current position of the camera
        Vector3 currentPosition = transform.position;

        // Calculate the target position
        float targetX = currentPosition.x;
        float targetY = currentPosition.y;

        float deltaX = target.position.x - currentPosition.x;
        float deltaY = target.position.y - currentPosition.y;

        // Move the camera horizontally if the player moves past the horizontal bounds
        if (Mathf.Abs(deltaX) > cameraBounds.x)
        {
            targetX = target.position.x - Mathf.Sign(deltaX) * cameraBounds.x;
        }

        // Move the camera vertically if the player moves past the vertical bounds
        if (Mathf.Abs(deltaY) > cameraBounds.y)
        {
            targetY = target.position.y - Mathf.Sign(deltaY) * cameraBounds.y;
        }

        // Clamp the camera position to ensure it doesn't go below the minimum height
        targetY = Mathf.Max(targetY, minCameraHeight);

        // Apply the offset
        targetX += offset.x;
        targetY += offset.y;

        // Smoothly move the camera to the target position using SmoothDamp
        Vector3 targetPosition = new Vector3(targetX, targetY, currentPosition.z);
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothSpeed);
    }
}
