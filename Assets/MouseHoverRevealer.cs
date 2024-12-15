using System.Collections;
using UnityEngine;

public class MouseHoverRevealer : MonoBehaviour
{
    public GameObject[] objectsToReveal;  // Objects to reveal
    public AudioClip hoverSound;          // Sound to play when hovering
    public float revealDelay = 0.5f;      // Delay before objects are revealed
    public float disappearDelay = 1f;     // Time before objects disappear after being revealed
    public LayerMask hoverLayer;          // LayerMask to ensure we're detecting the correct objects
    public float soundVolume = 1f;        // Volume control for the sound (0 to 1)

    private AudioSource audioSource;      // AudioSource to play the sound
    private bool hasTriggered = false;    // Tracks whether the action has already triggered

    private void Start()
    {
        // Ensure all objects are initially hidden
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Set up the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = hoverSound;
        audioSource.volume = soundVolume;  // Set the volume from the inspector
    }

    private void Update()
    {
        // Raycast to check if the mouse is over the collider
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, hoverLayer);

        if (hit.collider != null && !hasTriggered)
        {
            // Trigger actions only once for the entire session
            hasTriggered = true;

            // Play sound once
            if (hoverSound != null)
            {
                audioSource.Play();
            }

            // Reveal objects after the delay
            StartCoroutine(RevealObjects());
        }
    }

    private IEnumerator RevealObjects()
    {
        yield return new WaitForSeconds(revealDelay);  // Wait before revealing objects

        // Activate objects
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Wait before disappearing
        yield return new WaitForSeconds(disappearDelay);

        // Hide objects again after the delay
        foreach (GameObject obj in objectsToReveal)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}
