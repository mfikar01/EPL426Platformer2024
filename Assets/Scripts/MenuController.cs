using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    // This function loads a scene by name asynchronously
    public void LoadSceneAsync(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // While the scene is still loading, yield until it's done
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Optional: Add a Quit function
    public void QuitGame()
    {
        Debug.Log("Quit game clicked!");
        Application.Quit(); // Only works in a build
    }
}
