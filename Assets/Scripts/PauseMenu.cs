using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // The UI for the pause menu
    private bool isPaused = false;

    void Start()
    {
    pauseMenu.SetActive(false); // Deactivate on start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f; // Resume normal time speed
        isPaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze time
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure time isn't frozen when going to menu
        SceneManager.LoadScene("Title Screen"); // Replace "MainMenu" with your main menu scene's name
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
