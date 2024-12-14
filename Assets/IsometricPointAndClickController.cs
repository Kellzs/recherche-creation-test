using UnityEngine;

public class IsometricPointAndClickController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the player
    public LayerMask walkableLayer1; // Layer mask for walkable areas on Tilemap 1 (Ground)
    public LayerMask walkableLayer2; // Layer mask for walkable areas on Tilemap 2 (Elevated Platforms)
    public LayerMask walkableLayer3; // Layer mask for walkable areas on Tilemap 3 (High Platforms)
    public Collider2D finalCollider; // Reference to the final collider

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private bool canMove = true; // Flag to control player movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    void Update()
    {
        // Only allow movement if canMove is true
        if (canMove && Input.GetMouseButtonDown(0)) // When the left mouse button is clicked
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
        Vector3 targetPosition3D = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

        // Start moving the player to the target position with smooth Z movement
        StartCoroutine(MoveToPosition(targetPosition3D));
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        // Move towards the target position at the specified moveSpeed
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Smoothly interpolate between the current position and the target position
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
        // Raycast to check if the clicked position is within a walkable area on Tilemap 2
        RaycastHit2D hit2 = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, walkableLayer2);
        // Raycast to check if the clicked position is within a walkable area on Tilemap 3
        RaycastHit2D hit3 = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, walkableLayer3);

        // Check if a valid collider was hit in any of the layers (Tilemap 1, 2, or 3)
        if (hit1.collider != null || hit2.collider != null || hit3.collider != null)
        {
            return true; // Valid walkable area
        }

        return false; // No valid platform found
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == finalCollider) // When the player enters the final collider
        {
            canMove = false; // Disable movement
            Debug.Log("Player has entered the final collider. Movement disabled.");
        }
    }
}
