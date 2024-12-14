using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class FinalColliderHandler : MonoBehaviour
{
    public AudioClip exitSound; // The sound to play when the player enters the collider
    public float delayBeforeQuit = 2f; // Delay in seconds before quitting the scene
    private bool hasTriggered = false; // Ensure this only happens once
    private AudioSource audioSource;

    void Start()
    {
        // Create an AudioSource for playing the sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = exitSound;
        audioSource.playOnAwake = false; // We control when the sound plays
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Ensure it's the player triggering
        {
            hasTriggered = true; // Prevent multiple triggers
            StartCoroutine(HandleGameExit());
        }
    }

    IEnumerator HandleGameExit()
    {
        if (exitSound != null)
        {
            audioSource.Play(); // Play the exit sound
            yield return new WaitForSeconds(exitSound.length); // Wait for the sound to finish
        }
        else
        {
            yield return new WaitForSeconds(delayBeforeQuit); // Wait for the delay
        }

        // Quit the game (or load a new scene if running in the editor)
#if UNITY_EDITOR
        Debug.Log("Quitting game or exiting scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the scene for testing
#else
        Application.Quit(); // Quits the standalone application
#endif
    }
}
