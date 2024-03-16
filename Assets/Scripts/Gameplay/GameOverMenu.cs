using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenu; 
    public TextMeshProUGUI finalScoreText; 
    public ScoreManager scoreManager; 
    public GameObject pauseMenu;

    public AudioManager audioManager;
    public AudioSource gameOverMusicSource; 

    void Start()
    {
        gameOverMenu.SetActive(false); // Deactivate on start
        Time.timeScale = 1f; // Ensure normal time speed

        // Initialize the AudioSource if necessary
        if (gameOverMusicSource == null) 
        {
            gameOverMusicSource = gameObject.AddComponent<AudioSource>(); 
            gameOverMusicSource.loop = true; // Make it loop
        }
    }

    public void ShowGameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze time

        if (finalScoreText != null && scoreManager != null)
        {
            finalScoreText.text = "SCORE: " + scoreManager.currentScore.ToString();
        }

        GameManager.isGameOver = true;
        pauseMenu.SetActive(false);

         if (audioManager != null)
        {
        audioManager.PauseMusic();
        }

         // Start game over music 
        gameOverMusicSource.Play();
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 

        // Find the PlayerCarMovement script
        PlayerCarMovement playerCar = FindObjectOfType<PlayerCarMovement>(); 

        // Re-enable the game object, collider and renderer
        if (playerCar != null) 
        {
            playerCar.gameObject.SetActive(true); 
            playerCar.GetComponent<Collider2D>().enabled = true;
            playerCar.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("TitleScreen"); 
        GameManager.isGameOver = false;
    }
}
