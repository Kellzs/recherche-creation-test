using UnityEngine;
using System.Collections;

public class SoundApproach : MonoBehaviour
{
    public AudioClip soundClip;           // The sound to play
    public float maxDistance = 10f;       // Maximum distance where sound is audible
    public float fadeInDuration = 2f;     // Duration to fade the sound in
    public float volumeAtMaxDistance = 0.1f; // Volume when at max distance
    public float maxVolume = 1f;          // Maximum volume during the fade-in (adjustable in the inspector)

    private bool hasPlayed = false;       // To track if the sound has already played
    private AudioSource audioSource;      // The audio source that will play the sound

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed) // Only play if player enters and sound hasn't played yet
        {
            hasPlayed = true;

            // Create an AudioSource dynamically
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundClip;
            audioSource.loop = false;  // Don't loop the sound
            audioSource.playOnAwake = false;  // Don't play the sound automatically

            // Set initial volume to 0
            audioSource.volume = 0;
            audioSource.spatialBlend = 1; // Set to 3D sound

            // Start the fade-in process
            StartCoroutine(FadeInSound());
        }
    }

    IEnumerator FadeInSound()
    {
        // Play the sound after adding the AudioSource
        audioSource.Play();

        // Fade in the sound over time
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            // Gradually increase volume until it reaches maxVolume
            audioSource.volume = Mathf.Lerp(0, maxVolume, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = maxVolume; // Ensure it reaches the max volume at the end
    }

    void Update()
    {
        if (audioSource != null)
        {
            // Calculate the distance between the player and the sound source
            float distance = Vector2.Distance(transform.position, Camera.main.transform.position); // Assuming player is the camera

            // Calculate volume based on distance
            float volume = Mathf.Clamp01(1 - (distance / maxDistance)); // Max volume when close, decreases with distance
            audioSource.volume = Mathf.Lerp(volumeAtMaxDistance, maxVolume, volume);
        }
    }
}
