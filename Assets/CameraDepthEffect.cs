using UnityEngine;

public class CameraDepthEffect : MonoBehaviour
{
    public Transform player;            // Reference to the player
    public float zoomFactor = 1.5f;     // How much the camera zooms during the transition
    public float transitionSpeed = 2f; // Speed of the transition
    public float lowerPlatformHeight = -2f; // Y-position threshold for the lower platform

    private Camera cam;                 // Reference to the camera component
    private float defaultZoom;          // Original camera orthographic size
    private Vector3 defaultPosition;    // Original camera position
    private bool hasTransitioned = false; // Track if the camera has already transitioned

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultZoom = cam.orthographicSize;      // Save the original zoom level
        defaultPosition = transform.position;   // Save the original camera position
    }

    void Update()
    {
        // Trigger the transition only if the player moves down and hasn't transitioned yet
        if (player.position.y <= lowerPlatformHeight && !hasTransitioned)
        {
            StartCoroutine(TransitionEffect());
        }
    }

    System.Collections.IEnumerator TransitionEffect()
    {
        hasTransitioned = true;  // Mark the transition as done

        // Zoom in and move the camera slightly downward
        float targetZoom = defaultZoom / zoomFactor;
        Vector3 targetPosition = new Vector3(defaultPosition.x, defaultPosition.y - 1f, defaultPosition.z);

        while (cam.orthographicSize > targetZoom + 0.01f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * transitionSpeed);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            yield return null; // Wait for the next frame
        }

        // Ensure camera reaches exact values
        cam.orthographicSize = targetZoom;
        transform.position = targetPosition;

        // Pause briefly on the lower platform
        yield return new WaitForSeconds(0.5f);

        // Return camera to its original position
        while (cam.orthographicSize < defaultZoom - 0.01f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultZoom, Time.deltaTime * transitionSpeed);
            transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * transitionSpeed);
            yield return null; // Wait for the next frame
        }

        // Ensure camera resets to its original values
        cam.orthographicSize = defaultZoom;
        transform.position = defaultPosition;

        hasTransitioned = false;  // Reset the transition flag so the effect can happen again
    }
}
