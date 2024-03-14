using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    private string titleSceneName = "Title Screen"; // Add the name of your title screen

    // This function will be called when the "Back" UI element is clicked
    public void BackToTitleScreen() 
    {
        SceneManager.LoadScene(titleSceneName);
    }
}
