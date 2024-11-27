using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MouseLight : MonoBehaviour
{
    public Light2D mouseLight;   // Reference to the 2D light that will follow the mouse
    

    void Update()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Move the light to the mouse position
        mouseLight.transform.position = new Vector3(mousePosition.x, mousePosition.y, mouseLight.transform.position.z);
    }
}
