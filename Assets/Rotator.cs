using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed of rotation in degrees per second
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        // Calculate rotation amount based on time and speed
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Rotate the light around the X-axis
        transform.Rotate(0f, 0f, rotationStep);

        // Reset rotation to loop (optional, keeps rotation within a 0-360 range)
        if (transform.eulerAngles.z <= -360f)
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.z = 0f;
            transform.eulerAngles = currentRotation;
        }
    }
}
