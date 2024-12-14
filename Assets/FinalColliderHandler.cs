using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class FinalColliderHandler : MonoBehaviour
{
    public AudioClip exitSound; // The sound to play when the player enters the collider
    public float delayBeforeQuit = 2f; // Delay in seconds before quitting the scene
    public float lightFadeDuration = 2f; // Time in seconds for the light to fade out
    public float soundStopDelay = 0.5f; // Delay in seconds before stopping the sound after the sprite is invisible

    private bool hasTriggered = false; // Ensure this only happens once per collider
    private AudioSource audioSource;
    public PlayerLightController2D playerLightController; // Reference to the PlayerLightController

    void Start()
    {
        // Create an AudioSource for playing the sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = exitSound;
        audioSource.playOnAwake = false; // We control when the sound plays
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player triggering
        {
            if (!hasTriggered) // Only trigger once
            {
                hasTriggered = true; // Prevent multiple triggers in the same scene
                StartCoroutine(HandleGameExit());
            }
        }
    }

    IEnumerator HandleGameExit()
    {
        // Start fading out the player's light immediately upon trigger
        if (playerLightController != null)
        {
            playerLightController.FadeOutLightAndSprite(lightFadeDuration); // Fade out the light and sprite
        }

        // Play the exit sound while the light fades
        if (exitSound != null)
        {
            audioSource.Play(); // Play the exit sound
        }

        // Wait for the light fade-out to complete before quitting or transitioning
        float elapsedTime = 0f;

        // While the sprite is still fading out, check if the opacity has reached 0
        while (elapsedTime < lightFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (playerLightController.playerSprite != null && playerLightController.playerSprite.color.a <= 0f)
            {
                // Wait for the additional delay after the sprite is invisible
                yield return new WaitForSeconds(soundStopDelay); // Delay before stopping the sound
                audioSource.Stop(); // Stop the sound after the delay
                break;
            }
            yield return null;
        }

        // Wait for the sound to finish if it's still playing after the additional delay
        if (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(exitSound.length - elapsedTime);
        }

        // After everything is done, quit the game or load a new scene
#if UNITY_EDITOR
        Debug.Log("Quitting game or exiting scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the scene for testing
#else
        Application.Quit(); // Quits the standalone application
#endif
    }
}
