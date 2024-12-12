using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MouseLight : MonoBehaviour
{
    public Light2D mouseLight;               // Reference to the 2D light that follows the mouse
    public float lightIntensity = 1f;       // Base intensity of the 2D light
    public float flickerIntensityMin = 0.5f; // Minimum intensity during flicker
    public float flickerIntensityMax = 1.5f; // Maximum intensity during flicker
    public float lightRange = 5f;           // Range of the light

    public float flickerDuration = 0.2f;     // Duration of a single flicker
    public float flickerCooldown = 2f;       // Minimum time between flickers
    public float randomFlickerIntervalMin = 1f; // Minimum time before a random flicker
    public float randomFlickerIntervalMax = 5f; // Maximum time before a random flicker

    private bool isVisible = true;           // Tracks whether the light is visible
    private bool isFlickering = false;       // Tracks if the light is currently flickering
    private bool isHovering = false;         // Tracks if the mouse is hovering over an object

    private GameObject currentHoveredObject = null; // Tracks the object being hovered over

    void Update()
    {
        // Toggle the visibility of the mouse light when pressing Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isVisible = !isVisible; // Switch the visibility state
            mouseLight.enabled = isVisible; // Enable or disable the light
        }

        // Ensure the light follows the mouse position in the scene
        if (mouseLight != null && isVisible)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
            mouseLight.transform.position = new Vector3(mousePosition.x, mousePosition.y, mouseLight.transform.position.z);

            // Check if the mouse is hovering over a physical object
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null) // Hovering over an object
            {
                if (hit.collider.gameObject != currentHoveredObject)
                {
                    currentHoveredObject = hit.collider.gameObject; // Update the hovered object
                    isHovering = true; // Start tracking hover state
                    StartRandomFlicker(); // Start the random flicker logic
                }
            }
            else
            {
                isHovering = false; // Reset hover state
                currentHoveredObject = null; // Clear the hovered object
            }

            // Adjust light properties
            mouseLight.pointLightOuterRadius = lightRange;
        }
    }

    private void StartRandomFlicker()
    {
        if (!isHovering) return; // Only run this if hovering over an object
        if (isFlickering) return; // Prevent multiple flicker coroutines from starting

        StartCoroutine(RandomFlickerRoutine());
    }

    private IEnumerator RandomFlickerRoutine()
    {
        isFlickering = true;

        while (isHovering) // Flickering happens only while hovering
        {
            float randomInterval = Random.Range(randomFlickerIntervalMin, randomFlickerIntervalMax);
            yield return new WaitForSeconds(randomInterval); // Wait before next flicker

            if (!isHovering) break; // Stop flickering if the mouse is no longer hovering

            StartCoroutine(FlickerLight());
        }

        isFlickering = false;
    }

    private IEnumerator FlickerLight()
    {
        float elapsed = 0f;
        while (elapsed < flickerDuration)
        {
            if (mouseLight != null && isVisible)
            {
                // Randomly adjust the light intensity within the specified range
                mouseLight.intensity = Random.Range(flickerIntensityMin, flickerIntensityMax);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset light intensity after flicker ends
        if (mouseLight != null)
        {
            mouseLight.intensity = lightIntensity;
        }
    }
}
