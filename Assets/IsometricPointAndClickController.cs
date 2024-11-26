using UnityEngine;

public class IsometricPointAndClickController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the player
    public LayerMask walkableLayer1; // Layer mask for walkable areas on Tilemap 1 (Ground)
    public LayerMask walkableLayer2; // Layer mask for walkable areas on Tilemap 2 (Elevated Platforms)

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // When the left mouse button is clicked
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse click position in world space
            Debug.Log($"Clicked Position: {clickPosition}"); // Debugging the clicked position

            if (IsPointWithinWalkableArea(clickPosition)) // Check if the clicked position is walkable
            {
                MovePlayerToClickPosition(clickPosition); // Move the player to the clicked position
            }
        }
    }

    void MovePlayerToClickPosition(Vector2 targetPosition)
    {
        // Convert target position to Vector3 (keeping Z as 0 for 2D movement)
        Vector3 targetPosition3D = new Vector3(targetPosition.x, targetPosition.y, 0f);

        // Start moving the player to the target position
        StartCoroutine(MoveToPosition(targetPosition3D));
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        // Move towards the target position at the specified moveSpeed
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move the player
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Ensure the player reaches the exact target position
        transform.position = targetPosition;
    }

    bool IsPointWithinWalkableArea(Vector2 point)
    {
        // Raycast to check if the clicked position is within a walkable area on Tilemap 1
        RaycastHit2D hit1 = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, walkableLayer1);
        // Debugging the hit information for layer 1
        Debug.Log($"Raycast hit on Layer 1: {hit1.collider != null} | Position: {hit1.point}");

        // Raycast to check if the clicked position is within a walkable area on Tilemap 2
        RaycastHit2D hit2 = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, walkableLayer2);
        // Debugging the hit information for layer 2
        Debug.Log($"Raycast hit on Layer 2: {hit2.collider != null} | Position: {hit2.point}");

        // Check if a valid collider was hit in either layer (Tilemap 1 or Tilemap 2)
        if (hit1.collider != null || hit2.collider != null)
        {
            // Get the platform's position (whichever layer was hit first)
            Vector3 platformPosition = hit1.collider != null ? hit1.collider.transform.position : hit2.collider.transform.position;

            // Convert the click position to Vector3 (assuming Z = 0)
            Vector3 point3D = new Vector3(point.x, point.y, 0f);

            // Debug the platform position and Z comparison
            Debug.Log($"Platform Position: {platformPosition} | Click Position: {point3D}");

            // Check if the Z difference between the platform and the clicked position is within tolerance
            if (Mathf.Abs(platformPosition.z - point3D.z) < 1f) // Adjust Z tolerance if necessary
            {
                return true; // Valid walkable area
            }
        }

        return false; // No valid platform found
    }
}
