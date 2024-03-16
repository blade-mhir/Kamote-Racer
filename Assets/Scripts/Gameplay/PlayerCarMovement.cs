using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerCarMovement : MonoBehaviour
{
    [System.Serializable]
    public struct SpeedInterval
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    public SpeedInterval[] speedIntervals;
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public Vector3 respawnPosition;
    public float invulnerabilityDuration = 1f;

    private Rigidbody2D rb;
    [SerializeField] public int livesRemaining = 3;
 public int GetLivesRemaining() 
    {
        return livesRemaining; 
    }

public GameOverMenu gameOverMenu;

    private float currentSpeed; 
    public GameObject explosionEffectPrefab;
    public GameObject respawnEffectPrefab;

//TURBO BOOST PU
    [Header("Turbo Boost")]
    public int powerUpsNeededForTurbo = 3; 
    public float turboBoostDuration = 5f;
    public float turboBoostSpeedMultiplier = 2f; 

    private int powerUpsCollected = 0; 
    private bool isTurboActive = false;
    private float originalSpeed; 

//HEALTH PU
    [Header("Health Power-Up")]
    public int powerUpsNeededForHealth = 1; // You can set to 1 for immediate effect
    public int maxLives = 3; // Maximum lives for the car
    private int healthPowerUpsCollected = 0; 

// TIME WARP POWER-UP
    [Header("Time Warp")]
    public int powerUpsNeededForTimeWarp = 3; 
    public float timeWarpDuration = 5f;
    public float timeWarpSlowdownFactor = 0.5f;

    private int timeWarpPowerUpsCollected = 0; 
    private bool isTimeWarpActive = false;

    [Header("Score Multiplier")]
    public float multiplierDuration = 5f; 
    public float multiplierFactor = 2f; 
    public int powerUpsNeededForMultiplier = 3; 
    private int scoreMultiplierPowerUpsCollected = 0;

    

    private bool isMultiplierActive = false; 
    private float multiplierEndTime;

    // References to other scripts
    public TrackMovement trackMovement;
    public EnemyCarMovement[] enemyMovements; // Array for multiple enemies
    public PowerUpMovement[] powerUpMovements; // Array for multiple power-ups

    public ScoreManager scoreManager;



    // Add a reference to your PowerUpCounter script
    public PowerUpCounter powerUpCounter;

    // Audio for powerups
    public AudioSource powerUpAudio;
    public AudioSource healthAudio;

    private float timeSurvived = 0f;

    private float scoreScalingFactor = 0.2f;

    private int baseScore = 0;


void FixedUpdate()
{
    // Forcefully fix Y-position 
    transform.position = new Vector3(transform.position.x, respawnPosition.y, transform.position.z);
}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        respawnPosition = transform.position; 

        currentSpeed = speedIntervals[0].speed; // Set initial speed
    }

 void Update()
    {
        if (isTurboActive) 
        {
            currentSpeed = originalSpeed * turboBoostSpeedMultiplier;
        }
        else
        {
        for (int i = 0; i < speedIntervals.Length; i++)
        {
            if (Time.time >= speedIntervals[i].startTime && Time.time <= speedIntervals[i].endTime)
            {
                currentSpeed = speedIntervals[i].speed;
                break; 
            }
        }
        }

         if (isMultiplierActive)
        {
            if (Time.time >= multiplierEndTime)
            {
                isMultiplierActive = false;
                // Optionally, you might want to display a visual effect here
            }
        }

        // Find the correct speed interval
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * currentSpeed, 0f); 

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Update real-world time (unmodified by time warps)
        timeSurvived += Time.deltaTime; 

         // If the car is active, calculate the score as before
        if (gameObject.activeSelf)
        { 
            timeSurvived += Time.deltaTime; 

            // Calculate the raw score (without multiplier for now)
            int rawScore = (int)(timeSurvived * scoreScalingFactor * 5);

            // Calculate INCREMENT to the base score, considering multiplier
            int scoreIncrement =  (int)(rawScore * (multiplierFactor - 1));

            // Apply the INCREMENTED effect of the multiplier permanently
            baseScore += scoreIncrement;

            // Display the current score (always with the accumulated multiplier)
            int currentScore = (int)(rawScore * multiplierFactor); 
            scoreManager.UpdateScore(currentScore);
        } 

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TimeWarpPowerUp"))
        {
            // Update the Time Warp counter
            powerUpCounter.IncrementTimeWarpCounter();

            timeWarpPowerUpsCollected++;

            // Play sound effect
            AudioSource powerUpAudioSource = collision.gameObject.GetComponent<AudioSource>();
            if (powerUpAudio != null)
            powerUpAudio.Play();

            if (timeWarpPowerUpsCollected >= powerUpsNeededForTimeWarp)
            {
                timeWarpPowerUpsCollected = 0; 
                ActivateTimeWarp();

                // Reset the Time Warp counter
                powerUpCounter.ResetTimeWarpCounter(); 
            }

            Destroy(collision.gameObject); 
        }
         else if (collision.gameObject.CompareTag("ScoreMultiplyPowerUp"))
        {
            
             powerUpCounter.IncrementScoreMultiplierCounter();
            
            scoreMultiplierPowerUpsCollected++;

            // Play sound effect
            AudioSource powerUpAudioSource = collision.gameObject.GetComponent<AudioSource>();
            if (powerUpAudio != null) 
                powerUpAudio.Play();

            // Check if collected enough to activate
            if (scoreMultiplierPowerUpsCollected >= powerUpsNeededForMultiplier)
            {
                scoreMultiplierPowerUpsCollected = 0; // Reset the counter
                ActivateScoreMultiplier();

                // Reset the Score Multiplier counter in PowerUpCounter 
                powerUpCounter.ResetScoreMultiplierCounter();
            }

            // Destroy the power-up GameObject
            Destroy(collision.gameObject); 
        }
        else if (collision.gameObject.CompareTag("TurboPowerUp"))
        {
            // Update the Turbo counter
            powerUpCounter.IncrementTurboCounter();

            powerUpsCollected++;

            // Play sound effect
            AudioSource powerUpAudioSource = collision.gameObject.GetComponent<AudioSource>();
            if (powerUpAudio != null)
            powerUpAudio.Play();

            if (powerUpsCollected >= powerUpsNeededForTurbo)
            {
                powerUpsCollected = 0; // Reset the counter
                ActivateTurboBoost();

                // Reset the Turbo counter
            powerUpCounter.ResetTurboCounter();
            }
            // Destroy the power-up GameObject
            Destroy(collision.gameObject); 
        }
        else if (collision.gameObject.CompareTag("HealthPowerUp") && livesRemaining < maxLives)
        {
        healthPowerUpsCollected++;

        // Play sound effect
        AudioSource powerUpAudioSource = collision.gameObject.GetComponent<AudioSource>();
        if (healthAudio != null)
            healthAudio.Play();

        // Check if collecting the power-up will exceed the maximum health
        if (livesRemaining + 1 <= maxLives)
        {
            // Increment health only if it doesn't exceed the maximum
            livesRemaining++;
            Debug.Log("Life Replenished!");
            // You might want to add visual or sound effects here
        }

        Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            livesRemaining--;

            if (livesRemaining > 0)
            {
                Respawn();  
                StartCoroutine(InvulnerabilityPeriod());

                // Cancel power-ups on enemy collision:
                if (isTurboActive) 
                {
                    isTurboActive = false;
                    // You might want to  add a visual/audio effect here 
                }
                if (isTimeWarpActive) 
                {
                    isTimeWarpActive = false;
                    Time.timeScale = 1.0f; // Reset time scale

                    // Reset Enemy Speeds
                    foreach (EnemyCarMovement enemyMovement in enemyMovements) 
                    {
                        enemyMovement.ResetSpeedFromTimeWarp(); 
                    }

                    // Reset Power-Up Speeds
                    foreach (PowerUpMovement powerUpMovement in powerUpMovements)
                    {
                        powerUpMovement.ResetSpeedFromTimeWarp();
                    }
                }
            }
            else 
            {
               StartCoroutine(GameOverSequence()); 
            }
        }
        // Get the impact force on the Y axis
            float impactForceY = collision.relativeVelocity.y; 

            // If there's a Y impact force, apply a counter force 
            if (impactForceY != 0f)
            {
                Vector2 counterForce = new Vector2(0f, -impactForceY);
                rb.AddForce(counterForce, ForceMode2D.Impulse); 
            } 
    }

   void Respawn()
    {
        StartCoroutine(RespawnWithEffects());
    }

    IEnumerator RespawnWithEffects()
    {

         // 2. Play respawn effect (with slight delay if needed)
        if (respawnEffectPrefab != null)
        {
            GameObject respawnInstance = Instantiate(respawnEffectPrefab, respawnPosition, Quaternion.identity);

            // Play the respawn sound effect
            AudioSource respawnAudio = respawnInstance.GetComponent<AudioSource>();
            if (respawnAudio != null)
            {
                respawnAudio.Play();
            }

            // Adjust the delay if needed for timing with longer effects
            yield return new WaitForSeconds(0.3f);  

            Destroy(respawnInstance);
        }

        // 1. Respawn immediately at the respawn position
        transform.position = respawnPosition;

        // 3. Disable movement temporarily
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Reset any existing velocity
        GetComponent<Rigidbody2D>().isKinematic = true;  // Make the car non-reactive to physics

        yield return new WaitForSeconds(1f); // Wait for 1 second

        // 4. Re-enable player movement and physics interaction
        GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.SetActive(true); // Enable the car
    }


    IEnumerator InvulnerabilityPeriod()
    {
        // Make car temporarily invulnerable (e.g., by disabling its collider)
        GetComponent<Collider2D>().enabled = false; 

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Re-enable the collider
        GetComponent<Collider2D>().enabled = true; 
    }

    void ActivateTurboBoost()
    {
        isTurboActive = true;
        originalSpeed = currentSpeed; // Store the speed before the boost 
        StartCoroutine(TurboBoostTimer());
        StartCoroutine(powerUpCounter.DisplayTurboActivatedText());
    }

    IEnumerator TurboBoostTimer() 
    {
        yield return new WaitForSeconds(turboBoostDuration);
        isTurboActive = false; 
    }

    void ReplenishLife()
    {
        if (livesRemaining < maxLives) {
            livesRemaining++;
            Debug.Log("Life Replenished!");
            // You might want to add visual or sound effects here
        } 
    }

    void ActivateTimeWarp()
    {
        isTimeWarpActive = true;
        Time.timeScale = timeWarpSlowdownFactor;
        // Slowdown Track (Remember to adjust the TrackMovement script)

        // Slowdown Enemies
        foreach (EnemyCarMovement enemyMovement in enemyMovements) 
        {
            enemyMovement.UpdateSpeedForTimeWarp(timeWarpSlowdownFactor);
        }

        // Slowdown Power-ups
        foreach (PowerUpMovement powerUpMovement in powerUpMovements)
        {
            powerUpMovement.UpdateSpeedForTimeWarp(timeWarpSlowdownFactor);
        }

        StartCoroutine(TimeWarpTimer());
        StartCoroutine(powerUpCounter.DisplayTimeWarpActivatedText());
    }

    IEnumerator TimeWarpTimer() 
    {
        yield return new WaitForSeconds(timeWarpDuration);
        Time.timeScale = 1.0f;
        // Reset Speeds (Modify movement scripts here too)

        isTimeWarpActive = false; 
    }

    public void ActivateScoreMultiplier()
    {
        isMultiplierActive = true;
        multiplierEndTime = Time.time + multiplierDuration;
        StartCoroutine(powerUpCounter.DisplayScoreMultiplierActivatedText());
    }



    IEnumerator GameOverSequence() 
    {
            if (explosionEffectPrefab != null)
    {
        GameObject explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Get the AudioSource component of the explosion effect
        AudioSource explosionAudio = explosionInstance.GetComponent<AudioSource>();
        if (explosionAudio != null)
        {
            explosionAudio.Play();  // Play the sound
        }

        Destroy(explosionInstance, 0.5f); // Adjust the delay as needed
    }

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false; 

        yield return new WaitForSeconds(0.5f); // Wait for explosion to play 

        // Execute Game Over Logic
        if (gameOverMenu != null) 
        {
            gameOverMenu.ShowGameOver();
        }
        else 
        {
            Debug.LogError("GameOverMenu reference is missing!");
        }
    }

}
