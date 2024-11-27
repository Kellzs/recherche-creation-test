using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public float followSpeed = 5f;     // Speed at which the camera follows the player
    public Vector3 offset;             // Offset from the player's position (added to the camera position)

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned to the CameraFollowPlayer script.");
            return; // Early exit if the player is not assigned
        }

        // Set a default offset in case it's not set in the inspector
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0, 5, -10); // Default offset (adjust based on your needs)
        }
    }

    void LateUpdate()
    {
        if (player == null) return; // Safety check in case the player is not assigned

        // Calculate the target position with the offset
        Vector3 targetPosition = player.position + offset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
