using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public string startSceneName = "Level1"; 
    public string creditsSceneName = "Credits"; 

    // This function will be called when the "Start" UI element is clicked
    public void StartGame() 
    {
        SceneManager.LoadScene(startSceneName);
    }

    // This function will be called when the "Credits" UI element is clicked
    public void ShowCredits() 
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    // This function will be called when the "Quit" UI element is clicked
    public void QuitGame()
    {
        Application.Quit();
    }
}
