using UnityEngine;

public class AutoCameraOrbit : MonoBehaviour
{
    [Header("Target & Distance")]
    public Transform target;
    public float distance = 10.0f;
    public float height = 3.0f;

    [Header("Rotation Settings")]
    public float orbitSpeed = 15.0f; // Degrees per second

    private float currentAngle = 180f;

    void LateUpdate()
    {
        if (!target) return;

        // Increment angle based on time
        currentAngle += orbitSpeed * Time.deltaTime;

        // Keep angle within 0-360 range
        if (currentAngle >= 360.0f) currentAngle -= 360.0f;

        // Calculate the new position using trigonometry
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = target.position.x + Mathf.Sin(radians) * distance;
        float z = target.position.z + Mathf.Cos(radians) * distance;
        
        Vector3 newPosition = new Vector3(x, target.position.y + height, z);

        // Apply position and look at the target
        transform.position = newPosition;
        transform.LookAt(target.position);
    }
}