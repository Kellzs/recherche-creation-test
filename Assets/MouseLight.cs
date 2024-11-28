using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MouseLight : MonoBehaviour
{
    public Light2D mouseLight;       // Reference to the 2D light that follows the mouse
    public float lightIntensity = 1f; // The intensity of the 2D light
    public float lightRange = 5f;     // The range of the 2D light

    private bool isVisible = true;    // Tracks whether the light is visible

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

            // Adjust light properties (optional)
            mouseLight.intensity = lightIntensity;
            mouseLight.pointLightOuterRadius = lightRange;
        }
    }
}
