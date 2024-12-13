using System.Collections; // Required for IEnumerator
using UnityEngine;

public class ObjectRevealer : MonoBehaviour
{
    public GameObject[] objectsToReveal; // List of objects to reveal and hide
    public AudioClip revealSound;       // Sound to play on reveal
    public float soundVolume = 1.0f;    // Volume of the sound
    public float revealDelay = 0.2f;    // Delay before revealing objects
    public float hideDelay = 1.0f;      // Delay before starting the flicker effect
    public float flickerDuration = 0.5f; // Duration of the flickering effect
    public float flickerInterval = 0.1f; // Interval between flickers

    private AudioSource audioSource;    // Reference to the audio source
    private bool objectsRevealed = false; // Prevent multiple triggers

    private void Start()
    {
        // Add an AudioSource to play the sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = revealSound;
        audioSource.volume = soundVolume;

        // Ensure all objects to reveal are initially hidden
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null)
                obj.SetActive(false); // Hide the object
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player") && !objectsRevealed)
        {
            // Start the reveal process
            StartCoroutine(DelayRevealAndFlicker());
        }
    }

    private IEnumerator DelayRevealAndFlicker()
    {
        yield return new WaitForSeconds(revealDelay); // Wait for the specified delay before revealing
        objectsRevealed = true;

        // Reveal all objects
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null)
                obj.SetActive(true); // Make the object visible
        }

        // Play the reveal sound
        if (revealSound != null)
            audioSource.Play();

        // Wait before starting the flicker effect
        yield return new WaitForSeconds(hideDelay);

        // Start the flickering effect
        float flickerEndTime = Time.time + flickerDuration;
        while (Time.time < flickerEndTime)
        {
            // Toggle the visibility of each object
            foreach (GameObject obj in objectsToReveal)
            {
                if (obj != null)
                    obj.SetActive(!obj.activeSelf); // Toggle visibility
            }
            yield return new WaitForSeconds(flickerInterval);
        }

        // Ensure all objects are hidden after flickering
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null)
                obj.SetActive(false); // Hide the object
        }
    }
}
