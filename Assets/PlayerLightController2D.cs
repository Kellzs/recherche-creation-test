using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D

public class PlayerLightController2D : MonoBehaviour
{
    public Light2D playerLight;       // Reference to the 2D light that will follow the player
    public Transform player;          // Reference to the player's Transform
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer
    public AudioClip fadeOutSound;    // The sound to play when the sprite becomes invisible
    private AudioSource audioSource;  // Reference to the audio source

    public float lightIntensity = 1f; // The intensity of the 2D light
    public float lightRange = 5f;     // The range of the 2D light

    private bool isVisible = true;    // Tracks whether the player and light are visible

    void Start()
    {
        // Set up the AudioSource for playing the sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = fadeOutSound;
    }

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

    // Call this method to fade out the light and the sprite
    public void FadeOutLightAndSprite(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    // Coroutine to handle the fading effect
    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsedTime = 0f;
        float initialIntensity = playerLight.intensity;
        float initialAlpha = playerSprite.color.a;

        Color initialColor = playerSprite.color; // Store initial sprite color (e.g., original color)
        Color darkRedColor = new Color(0.5f, 0f, 0f); // Dark red color to fade to

        // Play the fade out sound as soon as the sprite becomes invisible
        bool soundPlayed = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Lerp the light intensity, sprite alpha, and sprite color
            playerLight.intensity = Mathf.Lerp(initialIntensity, 0f, t);
            playerSprite.color = Color.Lerp(initialColor, darkRedColor, t); // Change to dark red
            Color spriteColor = playerSprite.color;
            spriteColor.a = Mathf.Lerp(initialAlpha, 0f, t); // Fade out alpha as well
            playerSprite.color = spriteColor;

            // Check if the sprite is fully invisible and play the sound if it hasn't been played yet
            if (!soundPlayed && spriteColor.a <= 0f)
            {
                audioSource.Play(); // Play the sound
                soundPlayed = true;
            }

            yield return null;
        }

        // Ensure the light and sprite are fully invisible after the fade
        playerLight.intensity = 0f;
        Color finalColor = playerSprite.color;
        finalColor.a = 0f;
        playerSprite.color = finalColor;
    }
}
