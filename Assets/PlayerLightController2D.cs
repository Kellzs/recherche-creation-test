using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightController2D : MonoBehaviour
{
    public Light2D playerLight;       // Reference to the 2D light that will follow the player
    public Transform player;          // Reference to the player's Transform
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer
    public float lightIntensity = 1f; // The intensity of the 2D light
    public float lightRange = 5f;     // The range of the 2D light

    private bool isVisible = true;    // Tracks whether the player and light are visible

    void Update()
    {
        // Toggle visibility of the player and light when pressing Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isVisible = !isVisible; // Switch the visibility state
            playerLight.enabled = isVisible; // Enable or disable the light
            playerSprite.enabled = isVisible; // Enable or disable the player's sprite
        }

        // Ensure the light follows the player's position in the scene
        if (player != null && playerLight != null && isVisible)
        {
            // Set the light's position to the player's position, maintaining the Z-axis for 2D
            playerLight.transform.position = new Vector3(player.position.x, player.position.y, playerLight.transform.position.z);

            // Adjust light properties (optional)
            playerLight.intensity = lightIntensity;
            playerLight.pointLightOuterRadius = lightRange;
        }
    }
}
