using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management

public class SceneTransition : MonoBehaviour
{
    public string firstSceneName = "Scene1";  // Name of the first scene (ensure it's added in Build Settings)
    public string secondSceneName = "Scene2"; // Name of the second scene (ensure it's added in Build Settings)
    public float delayBeforeSwitch = 10f; // Delay in seconds before switching scenes

    void Start()
    {
        // Load the first scene when the game starts
        SceneManager.LoadScene(firstSceneName);

        // Start the scene transition after the delay
        Invoke("GoToNextScene", delayBeforeSwitch);
    }

    void GoToNextScene()
    {
        // Load the second scene after the delay
        SceneManager.LoadScene(secondSceneName);
    }
}
