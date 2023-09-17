using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static bool isGamePaused = false;

    public void PauseGame()
    {
        if(isGamePaused)
        {
            Resume();
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
