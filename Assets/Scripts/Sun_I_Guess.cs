using UnityEngine;

public class LightRotator : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed of rotation in degrees per second

    void Update()
    {
        // Calculate rotation amount based on time and speed
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Rotate the light around the X-axis
        transform.Rotate(rotationStep, 0f, 0f);

        // Reset rotation to loop (optional, keeps rotation within a 0-360 range)
        if (transform.eulerAngles.x >= 360f)
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = 0f;
            transform.eulerAngles = currentRotation;
        }
    }
}
