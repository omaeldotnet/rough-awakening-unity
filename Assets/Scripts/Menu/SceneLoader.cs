using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("World");
        Debug.Log("World is starting");
    }

    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("Settings");
        Debug.Log("Settings is starting");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu is starting");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
