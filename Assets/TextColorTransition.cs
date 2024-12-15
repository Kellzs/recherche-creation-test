using UnityEngine;
using TMPro;
using System.Collections;
public class TextColorTransition : MonoBehaviour
{
    public TMP_Text tmpText; // Assign your TextMeshPro object in the Inspector
    public Color startColor = Color.white; // Initial color
    public Color endColor = new Color(0.5f, 0, 0); // Dark red color (R=0.5, G=0, B=0)
    public float duration = 8f; // Duration of the color transition in seconds

    void Start()
    {
        if (tmpText != null)
        {
            // Start the color transition
            StartCoroutine(ChangeTextColor());
        }
    }

    private IEnumerator ChangeTextColor()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp between the start color and the end color
            tmpText.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final color is set after the loop
        tmpText.color = endColor;
    }
}
