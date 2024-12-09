using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("Movement Settings")]
    public float amplitude = 2f;    // The distance the object moves
    public float speed = 2f;        // The speed of the movement
    public bool moveUpDown = true;  // Toggle to choose movement direction (true = up/down, false = left/right)

    private Vector3 startPosition;

    void Start()
    {
        // Store the object's starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement based on sine wave
        float offset = Mathf.Sin(Time.time * speed) * amplitude;

        // Update position based on selected movement direction
        if (moveUpDown)
        {
            // Move up and down (Y-axis)
            transform.position = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);
        }
        else
        {
            // Move left and right (X-axis)
            transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
        }
    }
}
