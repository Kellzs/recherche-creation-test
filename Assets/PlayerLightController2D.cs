using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightController2D : MonoBehaviour
{
    public Light2D playerLight;       // Reference to the 2D light that will follow the player
    public Transform player;          // Reference to the player's Transform


    void Update()
    {
        // Ensure the light follows the player's position in the scene
        if (player != null && playerLight != null)
        {
            // Set the light's position to the player's position, maintaining the Z-axis for 2D
            playerLight.transform.position = new Vector3(player.position.x, player.position.y, playerLight.transform.position.z);

           
        }
    }
}
