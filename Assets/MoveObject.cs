using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float minY = 129f; // Minimum Y value
    public float maxY = 131.28f; // Maximum Y value
    public float speed = 1f; // Speed of movement

    private void Update()
    {
        // Calculate the Y position using Mathf.PingPong to oscillate between minY and maxY
        float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;

        // Apply the new Y position to the object
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
