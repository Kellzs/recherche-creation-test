using UnityEngine;
using System.Collections;

public class SoundApproach : MonoBehaviour
{
    public AudioClip soundClip;           // The first sound to play
    public AudioClip secondarySoundClip;  // The second sound to play
    public float maxDistance = 10f;       // Maximum distance where sound is audible
    public float fadeInDuration = 2f;     // Duration to fade the sound in
    public float volumeAtMaxDistance = 0.1f; // Volume when at max distance
    public float maxVolume = 1f;          // Maximum volume for the first sound
    public float secondaryMaxVolume = 0.5f; // Maximum volume for the second sound

    private bool hasPlayed = false;       // To track if the sound has already played
    private AudioSource primaryAudioSource; // The audio source that will play the first sound
    private AudioSource secondaryAudioSource; // The audio source that will play the second sound

    public PlayerLightController2D playerLightController; // Reference to PlayerLightController2D

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed) // Only play if player enters and sound hasn't played yet
        {
            hasPlayed = true;

            // Create the primary AudioSource dynamically
            primaryAudioSource = gameObject.AddComponent<AudioSource>();
            primaryAudioSource.clip = soundClip;
            primaryAudioSource.loop = false;  // Don't loop the sound
            primaryAudioSource.playOnAwake = false;  // Don't play the sound automatically
            primaryAudioSource.volume = 0;  // Start with volume 0
            primaryAudioSource.spatialBlend = 1; // Set to 3D sound

            // Create the secondary AudioSource dynamically (if the secondary sound is assigned)
            if (secondarySoundClip != null)
            {
                secondaryAudioSource = gameObject.AddComponent<AudioSource>();
                secondaryAudioSource.clip = secondarySoundClip;
                secondaryAudioSource.loop = false;  // Don't loop the sound
                secondaryAudioSource.playOnAwake = false;  // Don't play the sound automatically
                secondaryAudioSource.volume = 0;  // Start with volume 0
                secondaryAudioSource.spatialBlend = 1; // Set to 3D sound
            }

            // Start the fade-in process for both sounds
            StartCoroutine(FadeInSounds());

            // Start flickering the light red
            if (playerLightController != null)
            {
                playerLightController.StartFlickeringRed(0.1f);
            }
            else
            {
                Debug.LogError("PlayerLightController2D is not assigned!");
            }
        }
    }

    IEnumerator FadeInSounds()
    {
        // Play both sounds
        primaryAudioSource.Play();
        if (secondaryAudioSource != null)
        {
            secondaryAudioSource.Play();
        }

        // Fade in both sounds over time
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float t = elapsedTime / fadeInDuration;

            // Gradually increase volume of both audio sources
            primaryAudioSource.volume = Mathf.Lerp(0, maxVolume, t);
            if (secondaryAudioSource != null)
            {
                secondaryAudioSource.volume = Mathf.Lerp(0, secondaryMaxVolume, t);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the volumes reach their maximum levels at the end
        primaryAudioSource.volume = maxVolume;
        if (secondaryAudioSource != null)
        {
            secondaryAudioSource.volume = secondaryMaxVolume;
        }
    }

    void Update()
    {
        if (primaryAudioSource != null)
        {
            // Calculate the distance between the player and the sound source
            float distance = Vector2.Distance(transform.position, Camera.main.transform.position); // Assuming player is the camera

            // Calculate volume based on distance
            float volume = Mathf.Clamp01(1 - (distance / maxDistance)); // Max volume when close, decreases with distance
            primaryAudioSource.volume = Mathf.Lerp(volumeAtMaxDistance, maxVolume, volume);
            if (secondaryAudioSource != null)
            {
                secondaryAudioSource.volume = Mathf.Lerp(volumeAtMaxDistance, secondaryMaxVolume, volume);
            }
        }
    }
}
