using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    private string startSceneName = "Level 1"; 
    private string helpSceneName = "Help"; 

    // This function will be called when the "Start" UI element is clicked
    public void StartGame() 
    {
        SceneManager.LoadScene(startSceneName);
    }

    // This function will be called when the "Credits" UI element is clicked
    public void ShowHelp() 
    {
        SceneManager.LoadScene(helpSceneName);
    }

    // This function will be called when the "Quit" UI element is clicked
    public void QuitGame()
    {
        Application.Quit();
    }
}
